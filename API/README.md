# IARA Fishing Information System API

## Overview
This is the backend API for the IARA (Executive Agency for Fisheries and Aquaculture) fishing information system for Bulgaria.

## Prerequisites
- .NET 8.0 SDK
- PostgreSQL 12 or higher
- Visual Studio 2022 / VS Code / Rider (optional)

## Setup Instructions

### 1. Database Setup
Create a PostgreSQL database using the provided SQL script in the project documentation.

### 2. Environment Configuration
1. Copy `.env.example` to `.env` in the `API/Iara.API` directory
2. Update the `.env` file with your actual values:

```env
# Database Configuration
DB_HOST=your_database_host
DB_PORT=5432
DB_NAME=your_database_name
DB_USER=your_database_user
DB_PASSWORD=your_database_password

# JWT Configuration
JWT_SECRET=your_jwt_secret_at_least_32_characters_long
JWT_ISSUER=IaraAPI
JWT_AUDIENCE=IaraClient
JWT_EXPIRATION_HOURS=24

# CORS Configuration (comma-separated)
CORS_ALLOWED_ORIGINS=http://localhost:4200,http://localhost:3000

# Application Settings
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://localhost:5000;https://localhost:5001
```

### 3. Install Dependencies
```bash
cd API/Iara.API
dotnet restore
```

### 4. Run Migrations (if using EF Core migrations)
```bash
dotnet ef database update
```

### 5. Run the API
```bash
dotnet run
```

The API will be available at:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001
- Swagger UI: http://localhost:5000/swagger

## API Endpoints

### Authentication
- `POST /api/Auth/login` - User login

### Fishing Ships
- `GET /api/FishingShips` - Get all ships (with optional filters)
- `GET /api/FishingShips/{id}` - Get ship by ID
- `POST /api/FishingShips` - Create new ship (Admin only)
- `PUT /api/FishingShips/{id}` - Update ship (Admin only)
- `DELETE /api/FishingShips/{id}` - Delete ship (Admin only)

### Fishing Log Entries
- `GET /api/FishingLogEntries` - Get all log entries (with optional filters)
- `GET /api/FishingLogEntries/{id}` - Get log entry by ID
- `POST /api/FishingLogEntries` - Create new log entry
- `POST /api/FishingLogEntries/{id}/sign` - Sign a log entry
- `DELETE /api/FishingLogEntries/{id}` - Delete log entry (Admin only)

### Catch Compositions
- `GET /api/CatchCompositions/{id}` - Get catch composition by ID
- `GET /api/CatchCompositions/log-entry/{logEntryId}` - Get catches for a log entry
- `POST /api/CatchCompositions` - Create new catch composition
- `DELETE /api/CatchCompositions/{id}` - Delete catch composition (Admin only)

### Fishing Permits
- `GET /api/FishingPermits` - Get all permits (with optional filters)
- `GET /api/FishingPermits/{id}` - Get permit by ID
- `POST /api/FishingPermits` - Create new permit (Admin only)
- `DELETE /api/FishingPermits/{id}` - Delete permit (Admin only)

### Inspections
- `GET /api/Inspections` - Get all inspections (with optional filters)
- `GET /api/Inspections/{id}` - Get inspection by ID
- `POST /api/Inspections` - Create new inspection (Inspector/Admin)
- `POST /api/Inspections/{id}/process` - Mark inspection as processed (Admin only)
- `DELETE /api/Inspections/{id}` - Delete inspection (Admin only)

### Registrations
- `GET /api/Registrations` - Get all registrations
- `GET /api/Registrations/{id}` - Get registration by ID
- `POST /api/Registrations` - Create new registration (Admin only)
- `DELETE /api/Registrations/{id}` - Delete registration (Admin only)

### Ship Classification Logs
- `GET /api/ShipClassificationLogs/{id}` - Get classification log by ID
- `GET /api/ShipClassificationLogs/ship/{shipId}` - Get classification logs for a ship
- `POST /api/ShipClassificationLogs` - Create new classification log (Admin only)
- `DELETE /api/ShipClassificationLogs/{id}` - Delete classification log (Admin only)

## Authentication
The API uses JWT Bearer token authentication. To access protected endpoints:

1. Login via `/api/Auth/login` to receive a JWT token
2. Include the token in the Authorization header: `Authorization: Bearer <token>`

## User Roles
- **Admin**: Full access to all endpoints
- **Inspector**: Can create inspections and view data
- **User**: Can view data and manage their own log entries

## Security Notes
- Never commit the `.env` file to version control
- Use strong passwords for database and JWT secret
- In production, use HTTPS only
- Regularly rotate JWT secrets

## Troubleshooting

### Database Connection Issues
- Verify PostgreSQL is running
- Check database credentials in `.env`
- Ensure the database exists

### JWT Token Issues
- Verify JWT_SECRET is at least 32 characters
- Check token expiration settings

### CORS Issues
- Add your frontend URL to CORS_ALLOWED_ORIGINS in `.env`
