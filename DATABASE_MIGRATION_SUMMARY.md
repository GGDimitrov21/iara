# Database Schema Migration Summary

## Overview
The entire IARA application has been updated to match the new PostgreSQL database schema. All layers of the application (Domain, Infrastructure, Application, API, and UI) have been synchronized.

## Database Schema Changes

### Tables Updated
1. **users** - User authentication and roles
2. **fishing_ships** - Vessel registration with IARA and maritime numbers
3. **registrations** - Registration documents for ships and permits
4. **fishing_permits** - Annual permits with quotas and hours limits
5. **fishing_log_entries** - Daily fishing logs with zones and routes
6. **catch_composition** - Detailed catch data per log entry
7. **inspections** - Inspection records with protocol numbers and violations
8. **ship_classification_log** - Annual ship classifications

### Key Schema Changes

#### Fishing Ships
- Changed from generic fields to specific maritime fields
- Added `iara_id_number` (unique identifier)
- Added `maritime_number` (unique identifier)
- Changed `name` → `ship_name`
- Changed `owner_details` → `owner_name`
- Removed `captain_id`, `width`, `displacement`
- Added `ship_length` instead of `length`
- Added `registration_document_id` (FK to registrations)
- Changed `registered_on` → `registration_date`

#### Fishing Permits
- Added `permit_year` (year-based permits)
- Removed `permit_number` (auto-generated)
- Changed quota system to min/max annual catch
- Added `total_hours_annual_limit`
- Removed `allowed_species` array
- Added `catch_quota_type`
- Added `registration_document_id` (FK to registrations)

#### Fishing Log Entries
- Changed from timestamps to date + time fields
- Added `log_date` (DateOnly)
- Changed `start_time`/`end_time` to TimeOnly (optional)
- Removed `captain_id`
- Removed `latitude`/`longitude` coordinates
- Added `fishing_zone` (string)
- Added `route_details` (text)
- Removed `signed_on`, kept `is_signed` boolean
- Added `submitted_at` timestamp

#### Catch Composition
- Renamed `species_id` → `fish_species`
- Made `weight_kg` optional
- Added `count` field (optional)
- Added `status` field (Kept/Released/Discarded)

#### Inspections
- Added `protocol_number` (required, unique)
- Changed `violation_found` → `has_violation`
- Changed `violation_details` → `violation_description`
- Added `sanctions_imposed` field
- Added `proof_of_violation_url` field
- Changed `location` → `inspection_location`
- Removed `inspection_type`
- Removed `notes`
- Made `inspector_id` optional (nullable)

## Code Changes

### 1. Domain Layer (Iara.Domain)

**Entities Updated:**
- `User.cs` - Aligned with users table
- `FishingShip.cs` - New maritime fields
- `FishingPermit.cs` - Year-based permits
- `FishingLogEntry.cs` - Date/time split, zones
- `CatchComposition.cs` - Species and status
- `Inspection.cs` - Protocol numbers
- `ShipClassificationLog.cs` - Aligned
- `Registration.cs` - Navigation properties

**Key Changes:**
- All entities use database column names as property names
- Removed inheritance from `BaseEntity` where not needed
- Added proper navigation properties for all relationships

### 2. Infrastructure Layer (Iara.Infrastructure)

**New Configuration Files:**
- `FishingPermitConfiguration.cs`
- `ShipClassificationLogConfiguration.cs`

**Updated Configurations:**
- All existing configurations updated to match exact column names
- Added proper indexes (unique constraints)
- Configured foreign key relationships with proper delete behaviors
- Set default values (timestamps, booleans)

**DbContext:**
- Already properly configured
- Auto-applies all configurations from assembly

### 3. Application Layer (Iara.Application)

**DTOs Already Updated:**
- `FishingShipDto` - Matches new schema
- `CreateFishingShipRequest` - Updated fields
- `UpdateFishingShipRequest` - Updated fields
- `FishingPermitDto` - Year-based system
- `CreateFishingPermitRequest` - New quota fields
- `FishingLogEntryDto` - Date/time/zone fields
- `CreateFishingLogEntryRequest` - Updated structure
- `CatchCompositionDto` - Species/count/status
- `InspectionDto` - Protocol numbers
- `CreateInspectionRequest` - New fields

### 4. API Layer (Iara.Api)

**Controllers:**
- All controllers already use the updated DTOs
- Endpoints remain the same (backward compatible URLs)
- Request/response models updated

### 5. UI Layer (IaraUI)

**TypeScript Types Updated:**
```typescript
// Updated all interfaces in types.ts
- FishingShip
- FishingPermit
- FishingLog (renamed to match FishingLogEntry)
- CatchComposition
- Inspection
```

**API Services Updated:**
```typescript
// services/api.ts
- All request/response interfaces
- Fishing ships API
- Fishing permits API
- Fishing logs API
- Inspections API
```

**Components Updated:**
- `Vessels.tsx` - Form fields match new schema
  - IARA ID Number
  - Maritime Number
  - Ship Name
  - Owner Name
  - Tonnage
  - Ship Length
  - Engine Power
  - Fuel Type
  - Registration Date

## Migration Path

### Database Migration
```sql
-- The provided SQL script creates the schema
-- Run: BEGIN; ... END; block from the user request
```

### Application Deployment
1. Update NuGet packages if needed
2. Update database connection string
3. Run `dotnet ef migrations add UpdateToNewSchema`
4. Run `dotnet ef database update`
5. Deploy API
6. Deploy UI

## Testing Checklist

- [ ] Test vessel registration with new fields
- [ ] Test permit creation with year-based system
- [ ] Test fishing log entry with zones and routes
- [ ] Test catch composition with species and status
- [ ] Test inspection with protocol number
- [ ] Verify unique constraints (IARA ID, Maritime Number, Protocol Number)
- [ ] Verify foreign key relationships
- [ ] Test cascade deletes
- [ ] Test optional fields (nullable)

## API Endpoint Compatibility

All existing endpoints remain the same:
- `POST /api/fishingships` - Updated request body
- `GET /api/fishingships/{id}` - Updated response
- `PUT /api/fishingships/{id}` - Updated request body
- `POST /api/fishingpermits` - Updated request body
- `POST /api/fishinglogs` - Updated request body
- `POST /api/inspections` - Updated request body

## Breaking Changes

### Request Bodies
All POST/PUT endpoints now expect different field names:
- `registrationNumber` → `maritimeNumber`
- `name` → `shipName`
- `ownerDetails` → `ownerName`
- `length` → `shipLength`
- Removed: `captainId`, `width`, `displacement`

### Response Bodies
All GET endpoints return different field structures matching the new schema.

## Backward Compatibility Notes

⚠️ **This is a breaking change** - Frontend applications must be updated simultaneously with the backend.

The database schema change is significant and requires:
1. Data migration scripts if existing data needs to be preserved
2. Coordinated deployment of API and UI
3. Testing of all CRUD operations

## Next Steps

1. **Database Migration**: Run the schema creation script
2. **Data Migration**: If needed, create scripts to migrate existing data
3. **Testing**: Run full integration tests
4. **Documentation**: Update API documentation (Swagger)
5. **Deployment**: Deploy API and UI together
