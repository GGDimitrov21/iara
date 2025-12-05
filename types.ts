export interface FishingShip {
  id: string;
  name: string;
  regNumber: string;
  owner: string;
  captainId: string;
  length: number;
  width: number;
  tonnage: number;
  fuelType: string;
  enginePower: number;
  displacement: number;
}

export interface FishingPermit {
  id: string;
  permitNumber: string;
  vesselName: string;
  issueDate: string;
  expiryDate: string;
  status: 'Active' | 'Expired' | 'Suspended';
}

export interface FishingLog {
  id: string;
  vesselId: string;
  captainId: string;
  startTime: string;
  durationHours: number;
  latitude: string;
  longitude: string;
  catchDetails: {
    speciesId: string;
    weightKg: number;
  }[];
  unloadedHistory?: {
    date: string;
    species: string;
    quantity: number;
    location: string;
  }[];
}

export interface Inspection {
  id: string;
  date: string;
  inspectorName: string;
  vesselName: string;
  permitNumber: string;
  notes: string;
  violationType?: string;
  violationNotes?: string;
  actType?: string;
  actNotes?: string;
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