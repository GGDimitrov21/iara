# Quick Start Guide - IARA UI-API Integration

## 🚀 Start the Application

### 1. Start the API
```bash
cd Iara.Api
dotnet run
# API runs on http://localhost:5153
```

### 2. Start the UI
```bash
cd IaraUI
npm install  # First time only
npm run dev
# UI runs on http://localhost:5173
```

## 🔐 User Secrets Configuration

Your connection string is stored securely in user secrets:
```bash
cd Iara.Api
dotnet user-secrets set "DatabaseSettings:ConnectionString" "Host=localhost;Port=5432;Database=iara_db;Username=postgres;Password=admin;"
```

## 📡 How to Use the API in UI

Import and use the API services:

```typescript
import { authApi, fishingShipsApi, fishingPermitsApi, fishingLogsApi, inspectionsApi } from '../services/api';

// Login
const response = await authApi.login({ username, password });
localStorage.setItem('accessToken', response.accessToken);

// Create a vessel
const vessel = await fishingShipsApi.create({
  name: 'Sea Breeze',
  registrationNumber: 'BG-12345',
  ownerDetails: 'Ivan Petrov',
  captainId: 1,
  length: 15.5,
  width: 4.2,
  tonnage: 25,
  fuelType: 'Diesel',
  enginePower: 150,
  displacement: 30
});

// Get all vessels
const vessels = await fishingShipsApi.getAll();

// Get permits for a ship
const permits = await fishingPermitsApi.getByShipId(shipId);

// Create inspection
const inspection = await inspectionsApi.create({
  shipId: 1,
  inspectionType: 'Routine',
  location: 'Varna Port',
  notes: 'All good',
  violationFound: false
});
```

## 🗺️ All Available API Methods

### Auth
- `authApi.login(data)` → Login
- `authApi.register(data)` → Register (Admin only)
- `authApi.refreshToken(data)` → Refresh token
- `authApi.logout()` → Logout

### Fishing Ships
- `fishingShipsApi.getAll()` → Get all ships
- `fishingShipsApi.getById(id)` → Get ship
- `fishingShipsApi.create(data)` → Create ship
- `fishingShipsApi.update(id, data)` → Update ship
- `fishingShipsApi.delete(id)` → Delete ship

### Fishing Permits
- `fishingPermitsApi.getById(id)` → Get permit
- `fishingPermitsApi.getByShipId(shipId)` → Get permits by ship
- `fishingPermitsApi.create(data)` → Create permit
- `fishingPermitsApi.revoke(id)` → Revoke permit

### Fishing Logs
- `fishingLogsApi.getById(id)` → Get log
- `fishingLogsApi.getByShipId(shipId)` → Get logs by ship
- `fishingLogsApi.create(data)` → Create log
- `fishingLogsApi.sign(id)` → Sign log

### Inspections
- `inspectionsApi.getById(id)` → Get inspection
- `inspectionsApi.getByShipId(shipId)` → Get inspections
- `inspectionsApi.create(data)` → Create inspection
- `inspectionsApi.process(id)` → Process inspection

## 📂 File Locations

### UI Service Files
- `IaraUI/services/apiClient.ts` - Axios client with auth
- `IaraUI/services/api.ts` - All API methods
- `IaraUI/.env` - API URL configuration

### API Configuration
- `Iara.Api/appsettings.json` - CORS origins
- User secrets - Database connection string

## ✅ What's Already Done

- ✅ All API endpoints mapped to TypeScript services
- ✅ Automatic token refresh
- ✅ Error handling
- ✅ CORS configured for localhost:5173
- ✅ Login page integrated
- ✅ Vessels page integrated
- ✅ User secrets configured
- ✅ Environment variables set up

## 📋 TODO

To complete the integration for other pages:

1. **Install axios**: `cd IaraUI && npm install axios`
2. **Permits Page**: Use `fishingPermitsApi`
3. **Operations Page**: Use `fishingLogsApi`
4. **Inspections Page**: Use `inspectionsApi`
5. **Dashboard**: Fetch summary statistics

## 🔧 Configuration Files

### IaraUI/.env
```
VITE_API_BASE_URL=http://localhost:5153
```

### User Secrets (Iara.Api)
```
DatabaseSettings:ConnectionString = Host=localhost;Port=5432;Database=iara_db;Username=postgres;Password=admin;
ApiSettings:AllowedOrigins:0 = http://localhost:5173
```

## 🎯 Testing the Integration

1. Start both API and UI
2. Navigate to http://localhost:5173
3. Go to login page
4. Enter credentials (once you have a user in the database)
5. On success, you'll be redirected to dashboard
6. Try creating a vessel on the Vessels page

## 🐛 Troubleshooting

**CORS errors?**
- Check `appsettings.json` includes `http://localhost:5173`
- Check browser console for exact error

**401 Unauthorized?**
- Check if token is stored in localStorage
- Check if API is running
- Check database connection

**Cannot connect to API?**
- Verify API is running on port 5153
- Check `.env` has correct `VITE_API_BASE_URL`
