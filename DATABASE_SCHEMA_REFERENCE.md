# IARA Database Schema - Quick Reference

## Field Mappings (Old → New)

### Fishing Ships
| Old Field | New Field | Type | Notes |
|-----------|-----------|------|-------|
| Id | ShipId | int | Primary key |
| Name | ShipName | string | Required |
| RegistrationNumber | MaritimeNumber | string | Unique |
| - | IaraIdNumber | string | NEW - Unique |
| OwnerDetails | OwnerName | string | Simplified |
| CaptainId | - | - | REMOVED |
| Length | ShipLength | decimal | Renamed |
| Width | - | - | REMOVED |
| Tonnage | Tonnage | decimal | Same |
| EnginePower | EnginePower | decimal | Same |
| Displacement | - | - | REMOVED |
| FuelType | FuelType | string | Now optional |
| RegisteredOn | RegistrationDate | DateOnly | Renamed |
| - | RegistrationDocumentId | int? | NEW - FK |
| - | IsActive | bool | NEW |
| - | LastUpdated | DateTime | NEW |

### Fishing Permits
| Old Field | New Field | Type | Notes |
|-----------|-----------|------|-------|
| Id | PermitId | int | Primary key |
| ShipId | ShipId | int | Same |
| PermitNumber | - | - | REMOVED (auto-gen) |
| - | PermitYear | int | NEW - Required |
| PermitType | CatchQuotaType | string | Renamed, optional |
| ValidFrom | ValidFrom | DateOnly | Same |
| ValidTo | ValidUntil | DateOnly | Renamed |
| QuotaLimit | MaxAnnualCatch | decimal? | Changed |
| - | MinAnnualCatch | decimal? | NEW |
| - | TotalHoursAnnualLimit | decimal? | NEW |
| AllowedSpecies | - | - | REMOVED |
| IsActive | Status | string | Changed to status |
| IssuedOn | IssueDate | DateOnly | Renamed |
| RevokedOn | - | - | REMOVED |
| - | RegistrationDocumentId | int? | NEW - FK |

### Fishing Log Entries
| Old Field | New Field | Type | Notes |
|-----------|-----------|------|-------|
| Id | LogEntryId | long | Primary key |
| ShipId | ShipId | int | Same |
| CaptainId | - | - | REMOVED |
| StartTime | StartTime | TimeOnly? | DateTime→TimeOnly |
| EndTime | EndTime | TimeOnly? | DateTime→TimeOnly |
| - | LogDate | DateOnly | NEW - Required |
| DurationHours | - | - | REMOVED (calculated) |
| Latitude | - | - | REMOVED |
| Longitude | - | - | REMOVED |
| - | FishingZone | string | NEW |
| CatchDetails | CatchDetails | string | Same (text) |
| - | RouteDetails | string | NEW |
| IsSigned | IsSigned | bool | Same |
| SignedOn | - | - | REMOVED |
| CreatedAt | SubmittedAt | DateTime | Renamed |

### Catch Composition
| Old Field | New Field | Type | Notes |
|-----------|-----------|------|-------|
| Id | CatchId | long | Primary key |
| LogEntryId | LogEntryId | long | Same |
| SpeciesId | FishSpecies | string | Renamed |
| WeightKg | WeightKg | decimal? | Now optional |
| - | Count | int? | NEW |
| - | Status | string | NEW |

### Inspections
| Old Field | New Field | Type | Notes |
|-----------|-----------|------|-------|
| Id | InspectionId | int | Primary key |
| InspectorId | InspectorId | int? | Now optional |
| ShipId | ShipId | int | Same |
| InspectionType | - | - | REMOVED |
| InspectionDate | InspectionDate | DateTime | Same |
| Location | InspectionLocation | string | Renamed |
| Notes | - | - | REMOVED |
| - | ProtocolNumber | string | NEW - Required, Unique |
| ViolationFound | HasViolation | bool | Renamed |
| ViolationDetails | ViolationDescription | string | Renamed |
| - | SanctionsImposed | string | NEW |
| - | ProofOfViolationUrl | string | NEW |
| IsProcessed | IsProcessed | bool | Same |
| ProcessedOn | - | - | REMOVED |

## Example API Requests

### Create Fishing Ship
```json
POST /api/fishingships
{
  "iaraIdNumber": "IARA-001-2024",
  "maritimeNumber": "BG-VAR-12345",
  "shipName": "Sea Breeze",
  "ownerName": "Ivan Petrov",
  "tonnage": 25.50,
  "shipLength": 15.75,
  "enginePower": 150.00,
  "fuelType": "Diesel",
  "registrationDate": "2024-01-15"
}
```

### Create Fishing Permit
```json
POST /api/fishingpermits
{
  "shipId": 1,
  "permitYear": 2024,
  "validFrom": "2024-01-01",
  "validUntil": "2024-12-31",
  "catchQuotaType": "Commercial",
  "minAnnualCatch": 1000.00,
  "maxAnnualCatch": 5000.00,
  "totalHoursAnnualLimit": 2000.00
}
```

### Create Fishing Log Entry
```json
POST /api/fishinglogs
{
  "shipId": 1,
  "logDate": "2024-12-05",
  "startTime": "06:00:00",
  "endTime": "14:00:00",
  "fishingZone": "Black Sea Zone A",
  "catchDetails": "Good conditions, moderate catch",
  "routeDetails": "Departed from Varna port, fished 20nm east",
  "catchCompositions": [
    {
      "fishSpecies": "Sprat",
      "weightKg": 150.5,
      "count": null,
      "status": "Kept"
    },
    {
      "fishSpecies": "Mackerel",
      "weightKg": 75.25,
      "count": null,
      "status": "Kept"
    }
  ]
}
```

### Create Inspection
```json
POST /api/inspections
{
  "shipId": 1,
  "inspectionLocation": "Varna Port",
  "protocolNumber": "INS-2024-001",
  "hasViolation": false,
  "violationDescription": null,
  "sanctionsImposed": null,
  "proofOfViolationUrl": null
}
```

## Database Constraints

### Unique Constraints
- `users.username`
- `users.email`
- `fishing_ships.iara_id_number`
- `fishing_ships.maritime_number`
- `fishing_log_entries.(ship_id, log_date)` - One log per ship per day
- `inspections.protocol_number`
- `ship_classification_log.(ship_id, classification_year)`

### Required Fields
- All `*_id` primary keys
- All foreign keys marked as NOT NULL
- `fishing_ships`: iaraIdNumber, maritimeNumber, shipName, ownerName, tonnage, shipLength, enginePower, registrationDate
- `fishing_permits`: shipId, permitYear, issueDate, validFrom, validUntil, status
- `fishing_log_entries`: shipId, logDate
- `catch_composition`: logEntryId, fishSpecies
- `inspections`: shipId, protocolNumber, hasViolation

### Optional Fields
- Most descriptive text fields
- Nullable foreign keys (registrationDocumentId, inspectorId)
- Time fields in log entries
- Catch composition count/weight

## Common Queries

### Get all ships with active permits
```sql
SELECT s.*, COUNT(p.permit_id) as active_permits
FROM fishing_ships s
LEFT JOIN fishing_permits p ON s.ship_id = p.ship_id 
  AND p.status = 'Active'
  AND p.valid_until >= CURRENT_DATE
WHERE s.is_active = true
GROUP BY s.ship_id;
```

### Get total catch by species for a ship
```sql
SELECT cc.fish_species, SUM(cc.weight_kg) as total_weight
FROM catch_composition cc
JOIN fishing_log_entries fle ON cc.log_entry_id = fle.log_entry_id
WHERE fle.ship_id = 1
  AND fle.log_date >= '2024-01-01'
GROUP BY cc.fish_species
ORDER BY total_weight DESC;
```

### Get ships with violations
```sql
SELECT DISTINCT s.ship_name, s.iara_id_number, 
  COUNT(i.inspection_id) as violation_count
FROM fishing_ships s
JOIN inspections i ON s.ship_id = i.ship_id
WHERE i.has_violation = true
GROUP BY s.ship_id, s.ship_name, s.iara_id_number
ORDER BY violation_count DESC;
```
