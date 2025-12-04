# IARA Setup Guide

## Quick Start

### 1. Database Setup

Create PostgreSQL database and run the schema script:

```sql
CREATE DATABASE iara_db;
```

Then execute the full database creation script provided in your assignment.

### 2. Configure User Secrets

Navigate to the API project and set up secrets:

```powershell
cd Iara.Api

# Initialize user secrets (already done - UserSecretsId in csproj)

# Set database connection
dotnet user-secrets set "DatabaseSettings:ConnectionString" "Host=localhost;Port=5432;Database=iara_db;Username=postgres;Password=YourPassword"

# Set JWT configuration
dotnet user-secrets set "JwtSettings:Secret" "your-super-secret-jwt-key-minimum-32-characters-long"
dotnet user-secrets set "JwtSettings:Issuer" "https://iara.government.bg"
dotnet user-secrets set "JwtSettings:Audience" "https://iara.government.bg"
dotnet user-secrets set "JwtSettings:AccessTokenExpirationMinutes" "60"
dotnet user-secrets set "JwtSettings:RefreshTokenExpirationDays" "7"
```

### 3. Restore & Run

```powershell
# From solution root
dotnet restore
dotnet build

# Run the API
cd Iara.Api
dotnet run
```

### 4. Access Swagger UI

Open browser: `https://localhost:7041`

### 5. Create First Admin User

Use Swagger UI or curl:

```bash
POST /api/auth/register
{
  "username": "admin",
  "password": "Admin123!@#",
  "fullName": "System Administrator",
  "email": "admin@iara.gov.bg",
  "role": "Admin"
}
```

**Note**: First registration might need to be done without authorization, or create user directly in database.

## Testing the API

### 1. Login
```json
POST /api/auth/login
{
  "username": "admin",
  "password": "Admin123!@#"
}
```

Copy the `accessToken` from response.

### 2. Add Authorization Header

In Swagger UI, click "Authorize" button and enter:
```
Bearer <your-access-token>
```

### 3. Create a Fishing Ship

```json
POST /api/fishingships
{
  "iaraIdNumber": "BGR-12345",
  "maritimeNumber": "MT-001",
  "shipName": "Sea Hunter",
  "ownerName": "Ivan Petrov",
  "tonnage": 25.50,
  "shipLength": 12.5,
  "enginePower": 150.0,
  "fuelType": "Diesel",
  "registrationDate": "2024-01-15"
}
```

## User Secrets Location

Windows: `%APPDATA%\Microsoft\UserSecrets\f7e98a24-61ba-4b86-bf6c-04ca5f9c7922\secrets.json`

Linux/macOS: `~/.microsoft/usersecrets/f7e98a24-61ba-4b86-bf6c-04ca5f9c7922/secrets.json`
