# Unit Testing Guide for IARA API

## Quick Setup

### 1. Create Test Project

```powershell
cd e:\projectsRepos\iara\iara

# Create xUnit test project
dotnet new xunit -n Iara.Tests

# Add necessary packages
cd Iara.Tests
dotnet add package Moq --version 4.20.70
dotnet add package FluentAssertions --version 6.12.0
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 9.0.0

# Add project references
dotnet add reference ../Iara.Domain/Iara.Domain.csproj
dotnet add reference ../Iara.Application/Iara.Application.csproj
dotnet add reference ../Iara.Infrastructure/Iara.Infrastructure.csproj

# Add test project to solution
cd ..
dotnet sln add Iara.Tests/Iara.Tests.csproj
```

---

## Example Tests

### Service Tests (FishingShipService)

```csharp
using Xunit;
using Moq;
using FluentAssertions;
using Iara.Infrastructure.Services;
using Iara.Domain.Repositories;
using Iara.Domain.Entities;
using Iara.Application.DTOs.FishingShips;
using Microsoft.Extensions.Logging;

namespace Iara.Tests.Services;

public class FishingShipServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ILogger<FishingShipService>> _mockLogger;
    private readonly FishingShipService _service;

    public FishingShipServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockLogger = new Mock<ILogger<FishingShipService>>();
        _service = new FishingShipService(_mockUnitOfWork.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var request = new CreateFishingShipRequest
        {
            IaraIdNumber = "BGR0001234",
            MaritimeNumber = "MT-2024-001",
            ShipName = "Sea Explorer",
            OwnerName = "John Doe",
            Tonnage = 150.5m,
            ShipLength = 25.3m,
            EnginePower = 450,
            FuelType = "Diesel",
            RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1))
        };

        _mockUnitOfWork.Setup(u => u.FishingShips.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<FishingShip, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockUnitOfWork.Setup(u => u.FishingShips.AddAsync(It.IsAny<FishingShip>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.ShipName.Should().Be("Sea Explorer");
        result.Data.IaraIdNumber.Should().Be("BGR0001234");
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateIaraId_ReturnsFailure()
    {
        // Arrange
        var request = new CreateFishingShipRequest
        {
            IaraIdNumber = "BGR0001234",
            MaritimeNumber = "MT-2024-001",
            ShipName = "Sea Explorer",
            OwnerName = "John Doe",
            Tonnage = 150.5m,
            ShipLength = 25.3m,
            EnginePower = 450,
            FuelType = "Diesel",
            RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        _mockUnitOfWork.Setup(u => u.FishingShips.AnyAsync(It.Is<System.Linq.Expressions.Expression<System.Func<FishingShip, bool>>>(expr => true), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("already exists");
    }
}
```

---

### Repository Tests (with In-Memory Database)

```csharp
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Iara.Infrastructure.Persistence;
using Iara.Infrastructure.Repositories;
using Iara.Domain.Entities;

namespace Iara.Tests.Repositories;

public class FishingShipRepositoryTests : IDisposable
{
    private readonly IaraDbContext _context;
    private readonly FishingShipRepository _repository;

    public FishingShipRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<IaraDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new IaraDbContext(options);
        _repository = new FishingShipRepository(_context);
    }

    [Fact]
    public async Task GetByIaraIdAsync_WithExistingShip_ReturnsShip()
    {
        // Arrange
        var ship = new FishingShip
        {
            IaraIdNumber = "BGR0001234",
            MaritimeNumber = "MT-2024-001",
            ShipName = "Sea Explorer",
            OwnerName = "John Doe",
            Tonnage = 150.5m,
            ShipLength = 25.3m,
            EnginePower = 450,
            FuelType = "Diesel",
            RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow),
            IsActive = true
        };

        await _context.FishingShips.AddAsync(ship);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIaraIdAsync("BGR0001234");

        // Assert
        result.Should().NotBeNull();
        result!.ShipName.Should().Be("Sea Explorer");
    }

    [Fact]
    public async Task GetActiveShipsAsync_ReturnsOnlyActiveShips()
    {
        // Arrange
        var activeShip = new FishingShip
        {
            IaraIdNumber = "BGR0001234",
            MaritimeNumber = "MT-2024-001",
            ShipName = "Active Ship",
            OwnerName = "John Doe",
            Tonnage = 150.5m,
            ShipLength = 25.3m,
            EnginePower = 450,
            FuelType = "Diesel",
            RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow),
            IsActive = true
        };

        var inactiveShip = new FishingShip
        {
            IaraIdNumber = "BGR0001235",
            MaritimeNumber = "MT-2024-002",
            ShipName = "Inactive Ship",
            OwnerName = "Jane Doe",
            Tonnage = 120.5m,
            ShipLength = 20.3m,
            EnginePower = 350,
            FuelType = "Diesel",
            RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow),
            IsActive = false
        };

        await _context.FishingShips.AddRangeAsync(activeShip, inactiveShip);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetActiveShipsAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().ShipName.Should().Be("Active Ship");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
```

---

### Validator Tests

```csharp
using Xunit;
using FluentAssertions;
using FluentValidation.TestHelper;
using Iara.Application.DTOs.Auth;
using Iara.Application.Validators;

namespace Iara.Tests.Validators;

public class RegisterRequestValidatorTests
{
    private readonly RegisterRequestValidator _validator;

    public RegisterRequestValidatorTests()
    {
        _validator = new RegisterRequestValidator();
    }

    [Fact]
    public void Validate_WithValidData_ShouldNotHaveErrors()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!",
            FullName = "Test User",
            Role = "Viewer"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("ab")]  // Too short
    [InlineData("a b c")]  // Contains space
    [InlineData("user@name")]  // Invalid character
    public void Validate_WithInvalidUsername_ShouldHaveError(string username)
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = username,
            Email = "test@example.com",
            Password = "Password123!",
            FullName = "Test User",
            Role = "Viewer"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Theory]
    [InlineData("password")]  // No uppercase
    [InlineData("PASSWORD")]  // No lowercase
    [InlineData("Password")]  // No number
    [InlineData("Password123")]  // No special character
    [InlineData("Pass1!")]  // Too short
    public void Validate_WithWeakPassword_ShouldHaveError(string password)
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = password,
            FullName = "Test User",
            Role = "Viewer"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
```

---

### AuthService Tests (with Refresh Token Storage)

```csharp
using Xunit;
using Moq;
using FluentAssertions;
using Iara.Infrastructure.Security;
using Iara.Domain.Repositories;
using Iara.Domain.Entities;
using Iara.Application.DTOs.Auth;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace Iara.Tests.Security;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly JwtSettings _jwtSettings;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockTokenService = new Mock<ITokenService>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        _jwtSettings = new JwtSettings
        {
            Secret = "SuperSecretKeyForTestingPurposes123!",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            AccessTokenExpirationMinutes = 15,
            RefreshTokenExpirationDays = 7
        };

        var mockHttpContext = new Mock<HttpContext>();
        var mockConnection = new Mock<ConnectionInfo>();
        mockConnection.Setup(c => c.RemoteIpAddress).Returns(System.Net.IPAddress.Parse("127.0.0.1"));
        mockHttpContext.Setup(c => c.Connection).Returns(mockConnection.Object);
        _mockHttpContextAccessor.Setup(h => h.HttpContext).Returns(mockHttpContext.Object);

        _authService = new AuthService(
            _mockUnitOfWork.Object,
            _mockPasswordHasher.Object,
            _mockTokenService.Object,
            Options.Create(_jwtSettings),
            _mockHttpContextAccessor.Object
        );
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_StoresRefreshTokenInDatabase()
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = "testuser",
            Password = "Password123!"
        };

        var user = new User
        {
            UserId = 1,
            Username = "testuser",
            PasswordHash = "hashedpassword",
            FullName = "Test User",
            Role = UserRoles.Viewer,
            IsActive = true
        };

        _mockUnitOfWork.Setup(u => u.Users.GetByUsernameAsync(request.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockPasswordHasher.Setup(p => p.VerifyPassword(request.Password, user.PasswordHash))
            .Returns(true);

        _mockTokenService.Setup(t => t.GenerateAccessToken(user.UserId, user.Username, user.Role))
            .Returns("access-token");

        _mockTokenService.Setup(t => t.GenerateRefreshToken())
            .Returns("refresh-token");

        _mockUnitOfWork.Setup(u => u.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.AccessToken.Should().Be("access-token");
        result.Data.RefreshToken.Should().Be("refresh-token");

        _mockUnitOfWork.Verify(u => u.RefreshTokens.AddAsync(
            It.Is<RefreshToken>(rt => 
                rt.Token == "refresh-token" && 
                rt.UserId == user.UserId &&
                rt.CreatedByIp == "127.0.0.1"
            ), 
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
```

---

## Running Tests

```powershell
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "FullyQualifiedName~FishingShipServiceTests"

# Run tests matching pattern
dotnet test --filter "Name~CreateAsync"
```

---

## Test Organization

```
Iara.Tests/
├── Services/
│   ├── FishingShipServiceTests.cs
│   ├── FishingPermitServiceTests.cs
│   ├── FishingLogEntryServiceTests.cs
│   └── InspectionServiceTests.cs
├── Repositories/
│   ├── FishingShipRepositoryTests.cs
│   ├── FishingPermitRepositoryTests.cs
│   └── RefreshTokenRepositoryTests.cs
├── Validators/
│   ├── AuthValidatorsTests.cs
│   ├── FishingShipValidatorsTests.cs
│   └── InspectionValidatorsTests.cs
├── Security/
│   ├── AuthServiceTests.cs
│   ├── TokenServiceTests.cs
│   └── PasswordHasherTests.cs
├── Controllers/
│   ├── AuthControllerTests.cs
│   └── FishingShipsControllerTests.cs
└── Helpers/
    └── TestDbContextFactory.cs
```

---

## Code Coverage Goals

- **Domain Logic:** 100% (critical business rules)
- **Services:** 90% (all business paths)
- **Repositories:** 80% (core queries)
- **Validators:** 100% (all validation rules)
- **Controllers:** 70% (happy path + auth)

---

## Best Practices

1. **AAA Pattern:** Arrange, Act, Assert in every test
2. **One Assertion Per Test:** Focus on single behavior
3. **Descriptive Names:** `MethodName_Scenario_ExpectedResult`
4. **Independent Tests:** No shared state between tests
5. **Mock External Dependencies:** Use Moq for repositories, services
6. **In-Memory DB for Integration:** EF Core InMemory for repository tests
7. **FluentAssertions:** More readable assertions
8. **Theory Tests:** Use `[Theory]` and `[InlineData]` for multiple inputs

---

## Additional Packages (Optional)

```bash
# For integration testing
dotnet add package Microsoft.AspNetCore.Mvc.Testing

# For mocking HTTP context
dotnet add package Microsoft.AspNetCore.Http

# For snapshot testing
dotnet add package Verify.Xunit

# For better test output
dotnet add package XunitXml.TestLogger
```

---

## CI/CD Integration

### GitHub Actions Example

```yaml
name: .NET Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '9.0.x'
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"
      - name: Upload coverage
        uses: codecov/codecov-action@v3
```

---

## Next Steps

1. ✅ Create test project
2. ✅ Add packages and references
3. Write unit tests for services
4. Write unit tests for repositories
5. Write unit tests for validators
6. Write integration tests for controllers
7. Set up CI/CD pipeline
8. Aim for 80%+ code coverage

**Happy Testing! 🧪**
