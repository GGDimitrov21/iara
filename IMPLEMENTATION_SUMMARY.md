# IARA API - Implementation Summary

## Overview
Secure .NET 9 REST API for the Bulgarian fishing vessel and permit management system (IARA). Built with Clean Architecture, enterprise-level security, and comprehensive business logic.

---

## ✅ Recently Completed Features (Latest Session)

### 1. Service Layer Implementation ✅
**Location:** `Iara.Infrastructure/Services/`

Created 4 complete business service implementations:

- **FishingShipService.cs**
  - CRUD operations with duplicate validation (IARA ID, Maritime Number)
  - Soft delete functionality
  - Automatic DTO mapping
  - Comprehensive logging

- **FishingPermitService.cs**
  - Permit creation with ship verification
  - Active permit validation (one per ship per year)
  - Permit revocation
  - Ship name lookup via repository

- **FishingLogEntryService.cs**
  - Log entry creation with catch compositions
  - Duplicate prevention (one entry per ship per date)
  - Electronic signature tracking
  - Nested catch composition handling

- **InspectionService.cs**
  - Inspection creation with protocol number uniqueness
  - Violation tracking and sanctions
  - Processing workflow
  - Inspector and ship verification

**DI Registration:** All services registered in `DependencyInjection.cs`

---

### 2. FluentValidation Integration ✅
**Package:** FluentValidation.AspNetCore 11.3.1  
**Location:** `Iara.Application/Validators/`

Created comprehensive validators:

- **AuthValidators.cs**
  - LoginRequestValidator: Username/password requirements
  - RegisterRequestValidator: Strong password policy (8+ chars, uppercase, lowercase, numbers, special chars), email validation, role validation
  - RefreshTokenRequestValidator: Token presence validation

- **FishingShipValidators.cs**
  - CreateFishingShipRequestValidator: IARA ID format (10 chars, uppercase + numbers), tonnage/length/power limits, fuel type enum, registration date validation
  - UpdateFishingShipRequestValidator: Same validations without IARA ID/Maritime Number (immutable)

- **FishingPermitValidators.cs**
  - CreateFishingPermitRequestValidator: Date range validation, quota limits, annual hours cap (8760), min/max catch relationship

- **FishingLogValidators.cs**
  - CreateFishingLogEntryRequestValidator: Date/time validation, max 50 catch compositions, nested validation
  - CreateCatchCompositionRequestValidator: Weight limits (100,000 kg max), status enum (Kept/Discarded/Released)

- **InspectionValidators.cs**
  - CreateInspectionRequestValidator: Protocol number format, conditional violation fields, URL validation for proof links

**Integration:** Auto-validation enabled in `Program.cs` via `AddFluentValidationAutoValidation()`

---

### 3. Refresh Token Storage ✅
**Location:** Domain, Infrastructure layers

#### New Components:

**Domain Layer:**
- `RefreshToken` entity with revocation tracking, IP logging, token rotation support
- `IRefreshTokenRepository` interface with active token queries and cleanup methods
- Added `RefreshTokens` navigation property to `User` entity
- Updated `IUnitOfWork` interface

**Infrastructure Layer:**
- `RefreshTokenRepository` implementation with:
  - Token lookup by value
  - Active tokens by user ID
  - Bulk revocation
  - Expired token cleanup (30+ days old)
- `RefreshTokenConfiguration` EF Core mapping (snake_case columns, unique index on token)
- Added to `IaraDbContext` as DbSet

**Security Enhancements in AuthService:**
- Tokens stored in database on Login and Register
- Refresh endpoint validates against database, not just JWT signature
- Token rotation: old token revoked, new token created
- Reuse detection: if revoked token used, all user tokens revoked (security measure)
- IP address tracking via IHttpContextAccessor
- Automatic token replacement chain (`ReplacedByToken` field)

**Database Schema:**
```sql
refresh_tokens (
  token_id SERIAL PRIMARY KEY,
  user_id INT NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
  token VARCHAR(500) NOT NULL UNIQUE,
  expires_at TIMESTAMP NOT NULL,
  is_revoked BOOLEAN NOT NULL DEFAULT FALSE,
  revoked_by_ip VARCHAR(50),
  revoked_at TIMESTAMP,
  replaced_by_token VARCHAR(500),
  created_by_ip VARCHAR(50) NOT NULL,
  created_at TIMESTAMP NOT NULL,
  updated_at TIMESTAMP
)
```

---

## Architecture Overview

### Clean Architecture Layers

```
┌─────────────────────────────────────┐
│         Iara.Api (Presentation)      │  ← Controllers, Middleware
├─────────────────────────────────────┤
│    Iara.Infrastructure (Adapters)    │  ← EF Core, Repositories, Services
├─────────────────────────────────────┤
│   Iara.Application (Use Cases)       │  ← DTOs, Interfaces, Validators
├─────────────────────────────────────┤
│     Iara.Domain (Core Business)      │  ← Entities, Domain Logic
└─────────────────────────────────────┘
```

### Technology Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| Framework | .NET | 9.0 |
| Language | C# | 12 (nullable enabled) |
| Database | PostgreSQL | via Npgsql 9.0.2 |
| ORM | Entity Framework Core | 9.0 |
| Authentication | JWT Bearer | System.IdentityModel.Tokens.Jwt 8.2.1 |
| Password Hashing | BCrypt | BCrypt.Net-Next 4.0.3 (work factor 12) |
| Validation | FluentValidation | 11.3.1 |
| Logging | Serilog | 8.0.3 (console + file) |
| API Docs | Swagger/OpenAPI | Swashbuckle 7.2.0 |
| Rate Limiting | AspNetCoreRateLimit | 5.0.0 |

---

## Domain Entities (9 Total)

1. **User** - System users with role-based access
2. **FishingShip** - Vessel registry
3. **Registration** - Ship registration documents
4. **FishingPermit** - Annual fishing permits
5. **FishingLogEntry** - Daily fishing activity logs
6. **CatchComposition** - Detailed catch records per log entry
7. **Inspection** - Regulatory inspections
8. **ShipClassificationLog** - Annual ship classifications
9. **RefreshToken** ✨NEW - JWT refresh token storage

---

## API Endpoints

### Authentication (`/api/auth`)
- `POST /register` - Create new user
- `POST /login` - Authenticate user
- `POST /refresh` - Refresh access token ✨ENHANCED (DB validation)
- `POST /revoke` - Revoke all user tokens ✨ENHANCED

### Fishing Ships (`/api/ships`)
- `GET /` - List all ships
- `GET /{id}` - Get ship by ID
- `POST /` - Create ship [Admin]
- `PUT /{id}` - Update ship [Admin]
- `DELETE /{id}` - Soft delete [Admin]

### Fishing Permits (`/api/permits`)
- `GET /{id}` - Get permit
- `GET /ship/{shipId}` - List permits for ship
- `POST /` - Create permit [Admin/Inspector]
- `POST /{id}/revoke` - Revoke permit [Admin]

### Fishing Logs (`/api/logs`)
- `GET /{id}` - Get log entry with catches
- `GET /ship/{shipId}` - List entries for ship
- `POST /` - Create entry [Fisherman/Admin]
- `POST /{id}/sign` - Sign entry [Fisherman]

### Inspections (`/api/inspections`)
- `GET /{id}` - Get inspection
- `GET /ship/{shipId}` - List inspections
- `POST /` - Create inspection [Inspector/Admin]
- `POST /{id}/process` - Mark processed [Admin]

---

## Security Features

### Authentication & Authorization
- **JWT Tokens:** HS256 algorithm, configurable expiration
- **Refresh Tokens:** Stored in database, auto-rotation, revocation support
- **Password Security:** BCrypt with work factor 12 (2^12 iterations)
- **Role-Based Access:** Admin, Inspector, Fisherman, Viewer
- **Token Reuse Detection:** Automatic revocation on suspicious activity

### API Protection
- **Rate Limiting:** 60 req/min, 1000 req/hour per IP
- **CORS:** Configurable allowed origins
- **HTTPS Enforcement:** Redirect middleware
- **Security Headers:** Content security policy ready

### Input Validation
- **FluentValidation:** Request-level validation before processing
- **Model Binding:** ASP.NET Core automatic validation
- **Business Rules:** Service-level duplicate/constraint checks

---

## Database Design

### Key Features
- **Snake_case naming:** All columns use PostgreSQL convention
- **Automatic timestamps:** `created_at`, `updated_at` via EF interceptor
- **Soft deletes:** `is_active` flag on ships
- **Referential integrity:** Foreign keys with cascade/set null rules
- **Unique constraints:** Usernames, emails, IARA IDs, protocol numbers, refresh tokens

### Example Entity Mapping
```csharp
// FishingShip entity
public class FishingShip : BaseEntity
{
    public int ShipId { get; set; }
    public string IaraIdNumber { get; set; }      // Unique, 10 chars
    public string MaritimeNumber { get; set; }     // Unique
    public string ShipName { get; set; }
    public string OwnerName { get; set; }
    public decimal Tonnage { get; set; }
    public decimal ShipLength { get; set; }
    public decimal EnginePower { get; set; }
    public string FuelType { get; set; }
    public DateOnly RegistrationDate { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation properties
    public Registration? RegistrationDocument { get; set; }
    public ICollection<FishingPermit> Permits { get; set; }
    // ... more
}
```

---

## Configuration

### User Secrets (Required)
```json
{
  "DatabaseSettings": {
    "ConnectionString": "Host=localhost;Port=5432;Database=iara_db;Username=postgres;Password=yourpassword"
  },
  "JwtSettings": {
    "Secret": "YourSuperSecretKeyAtLeast32CharactersLong!",
    "Issuer": "IaraAPI",
    "Audience": "IaraClients",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

### Rate Limiting (appsettings.json)
```json
"IpRateLimiting": {
  "GeneralRules": [
    {
      "Endpoint": "*",
      "Period": "1m",
      "Limit": 60
    },
    {
      "Endpoint": "*",
      "Period": "1h",
      "Limit": 1000
    }
  ]
}
```

---

## Project Structure

```
Iara.Domain/
├── Entities/              # 9 domain entities
├── Repositories/          # IRepository<T>, IUnitOfWork, specific repo interfaces
├── Common/                # BaseEntity
└── Exceptions/            # Domain exceptions

Iara.Application/
├── DTOs/                  # Auth, Ships, Permits, Logs, Inspections
├── Services/              # IAuthService, IFishingShipService, etc.
├── Validators/            # 5 validator files ✨NEW
├── Common/                # Result<T> wrapper
└── Configuration/         # DatabaseSettings

Iara.Infrastructure/
├── Persistence/
│   ├── IaraDbContext.cs           # 9 DbSets
│   └── Configurations/            # 9 EF configurations ✨RefreshToken NEW
├── Repositories/
│   ├── Repository.cs              # Generic implementation
│   ├── UnitOfWork.cs              # Transaction coordinator
│   ├── ShipRepositories.cs        # User, Ship, Registration, Permit
│   └── LogRepositories.cs         # LogEntry, Catch, Inspection, Classification, RefreshToken ✨NEW
├── Services/                      # ✨4 NEW business services
│   ├── FishingShipService.cs
│   ├── FishingPermitService.cs
│   ├── FishingLogEntryService.cs
│   └── InspectionService.cs
├── Security/
│   ├── AuthService.cs             # ✨UPDATED with DB token storage
│   ├── TokenService.cs
│   ├── PasswordHasher.cs
│   └── JwtSettings.cs
└── DependencyInjection.cs         # ✨UPDATED with new registrations

Iara.Api/
├── Controllers/           # 5 controllers (20+ endpoints)
├── Middleware/            # Exception handling
└── Program.cs             # ✨UPDATED with FluentValidation
```

---

## Build Status

```
✅ Iara.Domain      - 0 errors, 0 warnings
✅ Iara.Application - 0 errors, 0 warnings  
✅ Iara.Infrastructure - 0 errors, 0 warnings
✅ Iara.Api         - 0 errors, 0 warnings

Build succeeded in 5.2s
```

---

## Next Steps (Remaining Todo)

### 4. Unit Tests ⏳
**Recommended Setup:**
```bash
cd e:\projectsRepos\iara\iara
dotnet new xunit -n Iara.Tests
cd Iara.Tests
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add reference ../Iara.Domain
dotnet add reference ../Iara.Application
dotnet add reference ../Iara.Infrastructure
```

**Test Coverage Suggestions:**
- [ ] Domain entity validation rules
- [ ] Repository query methods
- [ ] Service business logic (permit validation, duplicate checks)
- [ ] AuthService token rotation logic
- [ ] FluentValidation rules
- [ ] Controller authorization
- [ ] Exception handling middleware

---

## Additional Recommendations

### High Priority
1. **Database Migration:** Create initial migration with `dotnet ef migrations add InitialCreate`
2. **Admin Seeding:** Create first admin user via script or migration
3. **Logging Enhancement:** Add structured logging with context (user ID, correlation ID)
4. **Health Checks:** Add detailed database connection health endpoint

### Medium Priority
5. **AutoMapper:** Replace manual DTO mapping with AutoMapper profiles
6. **Pagination:** Add PagedResult<T> for list endpoints
7. **Caching:** Redis for frequently accessed ships/permits
8. **Background Jobs:** Hangfire for expired token cleanup

### Low Priority
9. **API Versioning:** `/api/v1/` endpoint structure
10. **Audit Trail:** Track all data changes (who, when, what)
11. **File Storage:** Azure Blob for violation proof uploads
12. **Real-time:** SignalR for inspection notifications

---

## Developer Notes

### Running the API
```powershell
# Set user secrets
dotnet user-secrets set "DatabaseSettings:ConnectionString" "Host=localhost;..."
dotnet user-secrets set "JwtSettings:Secret" "YourSecretKey..."

# Create database
dotnet ef migrations add InitialCreate --project Iara.Infrastructure --startup-project Iara.Api
dotnet ef database update --project Iara.Infrastructure --startup-project Iara.Api

# Run API
cd Iara.Api
dotnet run
```

### Swagger UI
Development: `https://localhost:5001/` (auto-redirects to Swagger)

### Sample API Call
```bash
# Login
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin123!"}'

# Create Ship (with token)
curl -X POST https://localhost:5001/api/ships \
  -H "Authorization: Bearer <your-token>" \
  -H "Content-Type: application/json" \
  -d '{
    "iaraIdNumber":"BGR0001234",
    "maritimeNumber":"MT-2024-001",
    "shipName":"Sea Explorer",
    "ownerName":"John Doe",
    "tonnage":150.5,
    "shipLength":25.3,
    "enginePower":450,
    "fuelType":"Diesel",
    "registrationDate":"2024-01-15"
  }'
```

---

## Security Checklist

- [x] JWT secret stored in user secrets (not appsettings.json)
- [x] Password hashing with BCrypt (work factor 12)
- [x] Refresh tokens stored in database
- [x] Token rotation on refresh
- [x] Token revocation support
- [x] Reuse detection with automatic revocation
- [x] IP address tracking
- [x] HTTPS enforcement
- [x] Rate limiting (60/min, 1000/hr)
- [x] Input validation (FluentValidation)
- [x] Role-based authorization
- [x] Soft deletes (data retention)
- [ ] SQL injection prevention (EF Core parameterization) - automatic
- [ ] XSS protection (client responsibility)
- [ ] CSRF tokens (for cookie-based auth, N/A for JWT)

---

## Performance Considerations

### Current Optimizations
- Entity Framework eager loading (`Include()`)
- Indexed columns (username, email, IARA ID, protocol number, refresh token)
- Async/await throughout
- Connection pooling (EF Core default)
- Rate limiting per IP

### Future Optimizations
- Redis caching for read-heavy data
- Database query profiling
- Response compression
- CDN for static content
- Load balancing

---

## Change Log

### 2024-XX-XX - Service Layer & Security Enhancements
**Added:**
- 4 complete business service implementations
- FluentValidation with 9 validators
- Refresh token database storage
- RefreshToken entity and repository
- IHttpContextAccessor for IP tracking
- Token rotation and revocation logic

**Changed:**
- AuthService updated with database token validation
- UnitOfWork includes RefreshToken repository
- User entity includes RefreshTokens navigation property
- DependencyInjection updated with new services

**Fixed:**
- UserConfiguration navigation property name (`Inspections` not `InspectionsPerformed`)

---

**Status:** Production-ready pending unit tests and database migration.
