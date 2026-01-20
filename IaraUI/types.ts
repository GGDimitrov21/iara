// Personnel
export interface Personnel {
  personId: number;
  name: string;
  role: string;
  contactEmail?: string;
}

// Vessel
export interface Vessel {
  vesselId: number;
  regNumber: string;
  vesselName: string;
  ownerDetails?: string;
  captainId?: number;
  captainName?: string;
  lengthM?: number;
  widthM?: number;
  tonnage?: number;
  fuelType?: string;
  enginePowerKw?: number;
  displacementTons?: number;
}

// Permit
export interface Permit {
  permitId: number;
  vesselId: number;
  vesselName: string;
  issueDate: string;
  expiryDate: string;
  isActive: boolean;
}

// Species
export interface Species {
  speciesId: number;
  speciesName: string;
}

// Catch Quota
export interface CatchQuota {
  quotaId: number;
  permitId: number;
  speciesId: number;
  speciesName: string;
  year: number;
  minCatchKg?: number;
  avgCatchKg?: number;
  maxCatchKg: number;
  fuelHoursLimit?: number;
}

// Logbook Entry
export interface LogbookEntry {
  logEntryId: number;
  vesselId: number;
  vesselName: string;
  captainId: number;
  captainName: string;
  startTime: string;
  durationHours?: number;
  latitude?: number;
  longitude?: number;
  speciesId: number;
  speciesName: string;
  catchKg: number;
}

// Inspection
export interface Inspection {
  inspectionId: number;
  vesselId: number;
  vesselName: string;
  inspectorId: number;
  inspectorName: string;
  inspectionDate: string;
  isLegal: boolean;
  notes?: string;
}