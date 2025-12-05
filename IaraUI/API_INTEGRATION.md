# API-UI Integration Guide

## Overview
This document describes the complete integration between the IARA API (.NET) and the IaraUI (React TypeScript).

## API Endpoints Mapped

### Authentication Endpoints
- **POST** `/api/auth/login` - User login
- **POST** `/api/auth/register` - User registration (Admin only)
- **POST** `/api/auth/refresh` - Refresh access token
- **POST** `/api/auth/revoke` - Logout/revoke tokens

### Fishing Ships Endpoints
- **GET** `/api/fishingships` - Get all ships
- **GET** `/api/fishingships/{id}` - Get ship by ID
- **POST** `/api/fishingships` - Create new ship
- **PUT** `/api/fishingships/{id}` - Update ship
- **DELETE** `/api/fishingships/{id}` - Delete ship

### Fishing Permits Endpoints
- **GET** `/api/fishingpermits/{id}` - Get permit by ID
- **GET** `/api/fishingpermits/ship/{shipId}` - Get permits by ship
- **POST** `/api/fishingpermits` - Create new permit
- **POST** `/api/fishingpermits/{id}/revoke` - Revoke permit

### Fishing Logs Endpoints
- **GET** `/api/fishinglogs/{id}` - Get log entry by ID
- **GET** `/api/fishinglogs/ship/{shipId}` - Get logs by ship
- **POST** `/api/fishinglogs` - Create new log entry
- **POST** `/api/fishinglogs/{id}/sign` - Sign log entry

### Inspections Endpoints
- **GET** `/api/inspections/{id}` - Get inspection by ID
- **GET** `/api/inspections/ship/{shipId}` - Get inspections by ship
- **POST** `/api/inspections` - Create new inspection
- **POST** `/api/inspections/{id}/process` - Process inspection

## Configuration

### API Configuration (.NET)

User secrets are configured for:
- `DatabaseSettings:ConnectionString` - PostgreSQL connection string
- `ApiSettings:BaseUrl` - API base URL (http://localhost:5000)
- `ApiSettings:AllowedOrigins:0` - CORS allowed origin (http://localhost:5173)

To set user secrets:
```bash
cd Iara.Api
dotnet user-secrets set "DatabaseSettings:ConnectionString" "Host=localhost;Port=5432;Database=iara_db;Username=postgres;Password=admin;"
dotnet user-secrets set "ApiSettings:AllowedOrigins:0" "http://localhost:5173"
```

### UI Configuration (React)

Environment variables in `.env`:
- `VITE_API_BASE_URL=http://localhost:5000`

## File Structure

### IaraUI/services/
- `apiClient.ts` - Axios instance with interceptors for auth and token refresh
- `api.ts` - Type-safe API service layer with all endpoint methods

### Updated Components
- `pages/Login.tsx` - Integrated with authApi.login()
- `pages/Vessels.tsx` - Integrated with fishingShipsApi.create()

## Authentication Flow

1. User submits login form
2. `authApi.login()` sends credentials to `/api/auth/login`
3. API returns accessToken, refreshToken, and user info
4. Tokens stored in localStorage
5. All subsequent requests include Bearer token
6. On 401 error, apiClient automatically attempts token refresh
7. If refresh fails, user redirected to login

## Usage Example

```typescript
import { authApi, fishingShipsApi } from '../services/api';

// Login
const response = await authApi.login({ username, password });
localStorage.setItem('accessToken', response.accessToken);

// Create vessel
const vessel = await fishingShipsApi.create({
  name: 'Sea Breeze',
  registrationNumber: 'BG-12345',
  // ... other fields
});
```

## Next Steps

1. Install axios in IaraUI: `npm install axios`
2. Update remaining pages (Permits, Operations, Inspections, etc.)
3. Add error handling and loading states
4. Implement protected routes
5. Add user role-based permissions in UI
