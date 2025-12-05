export interface FishingShip {
  shipId: number;
  iaraIdNumber: string;
  maritimeNumber: string;
  shipName: string;
  ownerName: string;
  tonnage: number;
  shipLength: number;
  enginePower: number;
  fuelType?: string;
  registrationDate: string;
  isActive: boolean;
}

export interface FishingPermit {
  permitId: number;
  shipId: number;
  shipName: string;
  permitYear: number;
  issueDate: string;
  validFrom: string;
  validUntil: string;
  catchQuotaType?: string;
  minAnnualCatch?: number;
  maxAnnualCatch?: number;
  totalHoursAnnualLimit?: number;
  status: string;
}

export interface CatchComposition {
  catchId: number;
  fishSpecies: string;
  weightKg?: number;
  count?: number;
  status?: string;
}

export interface FishingLog {
  logEntryId: number;
  shipId: number;
  shipName: string;
  logDate: string;
  startTime?: string;
  endTime?: string;
  fishingZone?: string;
  catchDetails?: string;
  routeDetails?: string;
  isSigned: boolean;
  submittedAt: string;
  catchCompositions: CatchComposition[];
}

export interface Inspection {
  inspectionId: number;
  inspectorId?: number;
  inspectorName?: string;
  shipId: number;
  shipName: string;
  inspectionDate: string;
  inspectionLocation?: string;
  protocolNumber: string;
  hasViolation: boolean;
  violationDescription?: string;
  sanctionsImposed?: string;
  proofOfViolationUrl?: string;
  isProcessed: boolean;
}

export interface CatchQuota {
  permitId: string;
  speciesName: string;
  year: number;
  minCatch: number;
  avgCatch: number;
  maxCatch: number;
  fuelLimit: number;
}