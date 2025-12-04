# IARA API - Quick Reference

## 🚀 Quick Start

```powershell
# 1. Configure secrets
cd e:\projectsRepos\iara\iara\Iara.Api
dotnet user-secrets set "DatabaseSettings:ConnectionString" "Host=localhost;Port=5432;Database=iara_db;Username=postgres;Password=yourpass"
dotnet user-secrets set "JwtSettings:Secret" "YourSuperSecretKeyAtLeast32CharactersLong!"
dotnet user-secrets set "JwtSettings:Issuer" "IaraAPI"
dotnet user-secrets set "JwtSettings:Audience" "IaraClients"

# 2. Create database
dotnet ef migrations add InitialCreate --project ../Iara.Infrastructure
dotnet ef database update --project ../Iara.Infrastructure

# 3. Run API
dotnet run
# API: https://localhost:5001
# Swagger: https://localhost:5001/
```

---

## 📁 Project Structure

```
Iara.Domain/          → Entities, Repository Interfaces, Domain Logic
Iara.Application/     → DTOs, Service Interfaces, Validators
Iara.Infrastructure/  → EF Core, Repositories, Services, Security
Iara.Api/             → Controllers, Middleware, Program.cs
```

---

## 🔐 Authentication Flow

```
1. Register/Login → Receive AccessToken + RefreshToken
2. Use AccessToken in Authorization header: Bearer <token>
3. When AccessToken expires → POST /api/auth/refresh with RefreshToken
4. Receive new AccessToken + RefreshToken pair
5. Old RefreshToken automatically revoked
```

---

## 🛡️ User Roles

| Role | Permissions |
|------|-------------|
| **Admin** | Full CRUD on all resources, user management |
| **Inspector** | Create inspections, view ships/permits |
| **Fisherman** | Create/sign log entries, view own ships |
| **Viewer** | Read-only access to public data |

---

## 📊 API Endpoints Summary

### Auth
- `POST /api/auth/register` - Create account
- `POST /api/auth/login` - Get tokens
- `POST /api/auth/refresh` - Refresh access token
- `POST /api/auth/revoke` - Logout (revoke all tokens)

### Ships
- `GET /api/ships` - List all ships
- `GET /api/ships/{id}` - Get ship details
- `POST /api/ships` - Create ship [Admin]
- `PUT /api/ships/{id}` - Update ship [Admin]
- `DELETE /api/ships/{id}` - Delete ship [Admin]

### Permits
- `GET /api/permits/{id}` - Get permit
- `GET /api/permits/ship/{shipId}` - List ship permits
- `POST /api/permits` - Issue permit [Admin/Inspector]
- `POST /api/permits/{id}/revoke` - Revoke permit [Admin]

### Logs
- `GET /api/logs/{id}` - Get log entry
- `GET /api/logs/ship/{shipId}` - List ship logs
- `POST /api/logs` - Create entry [Fisherman/Admin]
- `POST /api/logs/{id}/sign` - Sign entry [Fisherman]

### Inspections
- `GET /api/inspections/{id}` - Get inspection
- `GET /api/inspections/ship/{shipId}` - List ship inspections
- `POST /api/inspections` - Create inspection [Inspector/Admin]
- `POST /api/inspections/{id}/process` - Process [Admin]

---

## 🔧 Common Commands

### Build & Run
```powershell
dotnet build                    # Build solution
dotnet run --project Iara.Api   # Run API
dotnet test                     # Run tests
dotnet watch run                # Hot reload
```

### Database Migrations
```powershell
# Create migration
dotnet ef migrations add <MigrationName> --project Iara.Infrastructure --startup-project Iara.Api

# Update database
dotnet ef database update --project Iara.Infrastructure --startup-project Iara.Api

# Remove last migration
dotnet ef migrations remove --project Iara.Infrastructure --startup-project Iara.Api

# Generate SQL script
dotnet ef migrations script --project Iara.Infrastructure --startup-project Iara.Api --output migration.sql
```

### User Secrets
```powershell
# List all secrets
dotnet user-secrets list --project Iara.Api

# Set secret
dotnet user-secrets set "Key:SubKey" "Value" --project Iara.Api

# Remove secret
dotnet user-secrets remove "Key" --project Iara.Api

# Clear all secrets
dotnet user-secrets clear --project Iara.Api
```

---

## 📝 Sample Requests

### Register User
```json
POST /api/auth/register
{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "Password123!",
  "fullName": "John Doe",
  "role": "Fisherman"
}
```

### Login
```json
POST /api/auth/login
{
  "username": "johndoe",
  "password": "Password123!"
}

Response:
{
  "userId": 1,
  "username": "johndoe",
  "fullName": "John Doe",
  "role": "Fisherman",
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "CfDJ8KtcV...",
  "expiresAt": "2024-12-14T12:30:00Z"
}
```

### Create Ship
```json
POST /api/ships
Authorization: Bearer <token>

{
  "iaraIdNumber": "BGR0001234",
  "maritimeNumber": "MT-2024-001",
  "shipName": "Sea Explorer",
  "ownerName": "John Doe",
  "tonnage": 150.5,
  "shipLength": 25.3,
  "enginePower": 450,
  "fuelType": "Diesel",
  "registrationDate": "2024-01-15"
}
```

### Create Permit
```json
POST /api/permits
Authorization: Bearer <token>

{
  "shipId": 1,
  "permitYear": 2024,
  "validFrom": "2024-01-01",
  "validUntil": "2024-12-31",
  "catchQuotaType": "Annual Quota",
  "minAnnualCatch": 1000,
  "maxAnnualCatch": 50000,
  "totalHoursAnnualLimit": 2000
}
```

### Create Log Entry
```json
POST /api/logs
Authorization: Bearer <token>

{
  "shipId": 1,
  "logDate": "2024-12-14",
  "startTime": "06:00:00",
  "endTime": "14:00:00",
  "fishingZone": "Black Sea Zone A",
  "catchDetails": "Good weather, normal operations",
  "routeDetails": "Port Varna -> Fishing grounds 42.5N, 28.2E",
  "catchCompositions": [
    {
      "fishSpecies": "European anchovy",
      "weightKg": 250.5,
      "count": null,
      "status": "Kept"
    },
    {
      "fishSpecies": "Turbot",
      "weightKg": 45.2,
      "count": 12,
      "status": "Kept"
    }
  ]
}
```

### Create Inspection
```json
POST /api/inspections
Authorization: Bearer <token>

{
  "shipId": 1,
  "inspectionLocation": "Port Burgas",
  "protocolNumber": "INSP-2024-001",
  "hasViolation": true,
  "violationDescription": "Catch exceeds daily quota by 15kg",
  "sanctionsImposed": "Fine of 500 BGN",
  "proofOfViolationUrl": "https://storage.example.com/evidence/2024-001.pdf"
}
```

---

## ⚠️ Validation Rules

### Passwords
- Minimum 8 characters
- At least 1 uppercase letter
- At least 1 lowercase letter
- At least 1 number
- At least 1 special character

### IARA ID
- Exactly 10 characters
- Uppercase letters and numbers only
- Unique across all ships

### Dates
- Registration dates cannot be in future
- Permit valid_until must be after valid_from
- Log entry dates cannot be in future

### Catch Weights
- Must be greater than 0
- Maximum 100,000 kg per entry

---

## 🔍 Troubleshooting

### Build Errors
```powershell
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

### Database Connection
```powershell
# Test PostgreSQL connection
psql -h localhost -U postgres -d iara_db

# Check connection string in secrets
dotnet user-secrets list --project Iara.Api
```

### JWT Errors
- Ensure Secret is at least 32 characters
- Check token expiration times
- Verify Issuer/Audience match

### Rate Limiting
- Default: 60 requests/minute per IP
- Check `appsettings.json` → IpRateLimiting
- Disable for development if needed

---

## 📚 Key Files

| File | Purpose |
|------|---------|
| `Program.cs` | Middleware pipeline, DI configuration |
| `DependencyInjection.cs` | Infrastructure services registration |
| `IaraDbContext.cs` | EF Core database context |
| `appsettings.json` | Public configuration (rate limits, CORS) |
| `User Secrets` | Sensitive config (DB, JWT secrets) |

---

## 🧪 Testing

```powershell
# Create test project
dotnet new xunit -n Iara.Tests
cd Iara.Tests
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package Microsoft.EntityFrameworkCore.InMemory

# Run tests
dotnet test

# With coverage
dotnet test --collect:"XPlat Code Coverage"
```

---

## 📈 Performance Tips

1. **Enable Response Caching** for read-only endpoints
2. **Use Redis** for session storage in production
3. **Enable Response Compression** in Program.cs
4. **Add Database Indexes** on frequently queried columns
5. **Implement Pagination** for large result sets

---

## 🚨 Security Checklist

- [x] JWT secrets in user secrets (not appsettings)
- [x] BCrypt password hashing
- [x] Refresh tokens in database
- [x] Token rotation on refresh
- [x] HTTPS enforcement
- [x] Rate limiting enabled
- [x] Input validation (FluentValidation)
- [x] Role-based authorization
- [ ] CORS configured for production origins
- [ ] Audit logging for sensitive operations

---

## 📞 Support Resources

- **EF Core Docs:** https://learn.microsoft.com/ef/core/
- **ASP.NET Core:** https://learn.microsoft.com/aspnet/core/
- **FluentValidation:** https://docs.fluentvalidation.net/
- **PostgreSQL:** https://www.postgresql.org/docs/

---

**Last Updated:** 2024-12-14  
**API Version:** 1.0  
**Framework:** .NET 9.0
