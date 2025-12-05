import apiClient from './apiClient';

// ==================== AUTH API ====================

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  role: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  userId: number;
  username: string;
  email: string;
  role: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

export const authApi = {
  login: async (data: LoginRequest): Promise<AuthResponse> => {
    const response = await apiClient.post<AuthResponse>('/api/auth/login', data);
    return response.data;
  },

  register: async (data: RegisterRequest): Promise<AuthResponse> => {
    const response = await apiClient.post<AuthResponse>('/api/auth/register', data);
    return response.data;
  },

  refreshToken: async (data: RefreshTokenRequest): Promise<AuthResponse> => {
    const response = await apiClient.post<AuthResponse>('/api/auth/refresh', data);
    return response.data;
  },

  logout: async (): Promise<void> => {
    await apiClient.post('/api/auth/revoke');
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
  },
};

// ==================== FISHING SHIPS API ====================

export interface CreateFishingShipRequest {
  iaraIdNumber: string;
  maritimeNumber: string;
  shipName: string;
  ownerName: string;
  tonnage: number;
  shipLength: number;
  enginePower: number;
  fuelType?: string;
  registrationDate: string; // YYYY-MM-DD
}

export interface UpdateFishingShipRequest {
  shipName: string;
  ownerName: string;
  tonnage: number;
  shipLength: number;
  enginePower: number;
  fuelType?: string;
  isActive: boolean;
}

export interface FishingShipResponse extends CreateFishingShipRequest {
  shipId: number;
  isActive: boolean;
}

export const fishingShipsApi = {
  getAll: async (): Promise<FishingShipResponse[]> => {
    const response = await apiClient.get<FishingShipResponse[]>('/api/fishingships');
    return response.data;
  },

  getById: async (id: number): Promise<FishingShipResponse> => {
    const response = await apiClient.get<FishingShipResponse>(`/api/fishingships/${id}`);
    return response.data;
  },

  create: async (data: CreateFishingShipRequest): Promise<FishingShipResponse> => {
    const response = await apiClient.post<FishingShipResponse>('/api/fishingships', data);
    return response.data;
  },

  update: async (id: number, data: UpdateFishingShipRequest): Promise<FishingShipResponse> => {
    const response = await apiClient.put<FishingShipResponse>(`/api/fishingships/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/api/fishingships/${id}`);
  },
};

// ==================== FISHING PERMITS API ====================

export interface CreateFishingPermitRequest {
  shipId: number;
  permitYear: number;
  validFrom: string; // YYYY-MM-DD
  validUntil: string; // YYYY-MM-DD
  catchQuotaType?: string;
  minAnnualCatch?: number;
  maxAnnualCatch?: number;
  totalHoursAnnualLimit?: number;
}

export interface FishingPermitResponse extends CreateFishingPermitRequest {
  permitId: number;
  shipName: string;
  issueDate: string; // YYYY-MM-DD
  status: string;
}

export const fishingPermitsApi = {
  getById: async (id: number): Promise<FishingPermitResponse> => {
    const response = await apiClient.get<FishingPermitResponse>(`/api/fishingpermits/${id}`);
    return response.data;
  },

  getByShipId: async (shipId: number): Promise<FishingPermitResponse[]> => {
    const response = await apiClient.get<FishingPermitResponse[]>(`/api/fishingpermits/ship/${shipId}`);
    return response.data;
  },

  create: async (data: CreateFishingPermitRequest): Promise<FishingPermitResponse> => {
    const response = await apiClient.post<FishingPermitResponse>('/api/fishingpermits', data);
    return response.data;
  },

  revoke: async (id: number): Promise<void> => {
    await apiClient.post(`/api/fishingpermits/${id}/revoke`);
  },
};

// ==================== FISHING LOGS API ====================

export interface CatchCompositionRequest {
  fishSpecies: string;
  weightKg?: number;
  count?: number;
  status?: string;
}

export interface CreateFishingLogEntryRequest {
  shipId: number;
  logDate: string; // YYYY-MM-DD
  startTime?: string; // HH:mm:ss
  endTime?: string; // HH:mm:ss
  fishingZone?: string;
  catchDetails?: string;
  routeDetails?: string;
  catchCompositions: CatchCompositionRequest[];
}

export interface CatchCompositionResponse extends CatchCompositionRequest {
  catchId: number;
}

export interface FishingLogEntryResponse {
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
  catchCompositions: CatchCompositionResponse[];
}

export const fishingLogsApi = {
  getById: async (id: number): Promise<FishingLogEntryResponse> => {
    const response = await apiClient.get<FishingLogEntryResponse>(`/api/fishinglogs/${id}`);
    return response.data;
  },

  getByShipId: async (shipId: number): Promise<FishingLogEntryResponse[]> => {
    const response = await apiClient.get<FishingLogEntryResponse[]>(`/api/fishinglogs/ship/${shipId}`);
    return response.data;
  },

  create: async (data: CreateFishingLogEntryRequest): Promise<FishingLogEntryResponse> => {
    const response = await apiClient.post<FishingLogEntryResponse>('/api/fishinglogs', data);
    return response.data;
  },

  sign: async (id: number): Promise<void> => {
    await apiClient.post(`/api/fishinglogs/${id}/sign`);
  },
};

// ==================== INSPECTIONS API ====================

export interface CreateInspectionRequest {
  shipId: number;
  inspectionLocation?: string;
  protocolNumber: string;
  hasViolation: boolean;
  violationDescription?: string;
  sanctionsImposed?: string;
  proofOfViolationUrl?: string;
}

export interface InspectionResponse {
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

export const inspectionsApi = {
  getById: async (id: number): Promise<InspectionResponse> => {
    const response = await apiClient.get<InspectionResponse>(`/api/inspections/${id}`);
    return response.data;
  },

  getByShipId: async (shipId: number): Promise<InspectionResponse[]> => {
    const response = await apiClient.get<InspectionResponse[]>(`/api/inspections/ship/${shipId}`);
    return response.data;
  },

  create: async (data: CreateInspectionRequest): Promise<InspectionResponse> => {
    const response = await apiClient.post<InspectionResponse>('/api/inspections', data);
    return response.data;
  },

  process: async (id: number): Promise<void> => {
    await apiClient.post(`/api/inspections/${id}/process`);
  },
};

// ==================== EXPORT ALL ====================

export default {
  auth: authApi,
  fishingShips: fishingShipsApi,
  fishingPermits: fishingPermitsApi,
  fishingLogs: fishingLogsApi,
  inspections: inspectionsApi,
};
import apiClient from './apiClient';
import { 
  FishingShip, 
  FishingPermit, 
  FishingLog, 
  Inspection 
} from '../types';

// ==================== AUTH API ====================

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  role: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  userId: number;
  username: string;
  email: string;
  role: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

export const authApi = {
  login: async (data: LoginRequest): Promise<AuthResponse> => {
    const response = await apiClient.post<AuthResponse>('/api/auth/login', data);
    return response.data;
  },

  register: async (data: RegisterRequest): Promise<AuthResponse> => {
    const response = await apiClient.post<AuthResponse>('/api/auth/register', data);
    return response.data;
  },

  refreshToken: async (data: RefreshTokenRequest): Promise<AuthResponse> => {
    const response = await apiClient.post<AuthResponse>('/api/auth/refresh', data);
    return response.data;
  },

  logout: async (): Promise<void> => {
    await apiClient.post('/api/auth/revoke');
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
  },
};

// ==================== FISHING SHIPS API ====================

export interface CreateFishingShipRequest {
  iaraIdNumber: string;
  maritimeNumber: string;
  shipName: string;
  ownerName: string;
  tonnage: number;
  shipLength: number;
  enginePower: number;
  fuelType?: string;
  registrationDate: string;
}

export interface UpdateFishingShipRequest {
  shipName: string;
  ownerName: string;
  tonnage: number;
  shipLength: number;
  enginePower: number;
  fuelType?: string;
  isActive: boolean;
}

export interface FishingShipResponse {
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

export const fishingShipsApi = {
  getAll: async (): Promise<FishingShipResponse[]> => {
    const response = await apiClient.get<FishingShipResponse[]>('/api/fishingships');
    return response.data;
  },

  getById: async (id: number): Promise<FishingShipResponse> => {
    const response = await apiClient.get<FishingShipResponse>(`/api/fishingships/${id}`);
    return response.data;
  },

  create: async (data: CreateFishingShipRequest): Promise<FishingShipResponse> => {
    const response = await apiClient.post<FishingShipResponse>('/api/fishingships', data);
    return response.data;
  },

  update: async (id: number, data: UpdateFishingShipRequest): Promise<FishingShipResponse> => {
    const response = await apiClient.put<FishingShipResponse>(`/api/fishingships/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/api/fishingships/${id}`);
  },
};

// ==================== FISHING PERMITS API ====================

export interface CreateFishingPermitRequest {
  shipId: number;
  permitYear: number;
  validFrom: string;
  validUntil: string;
  catchQuotaType?: string;
  minAnnualCatch?: number;
  maxAnnualCatch?: number;
  totalHoursAnnualLimit?: number;
}

export interface FishingPermitResponse {
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

export const fishingPermitsApi = {
  getById: async (id: number): Promise<FishingPermitResponse> => {
    const response = await apiClient.get<FishingPermitResponse>(`/api/fishingpermits/${id}`);
    return response.data;
  },

  getByShipId: async (shipId: number): Promise<FishingPermitResponse[]> => {
    const response = await apiClient.get<FishingPermitResponse[]>(`/api/fishingpermits/ship/${shipId}`);
    return response.data;
  },

  create: async (data: CreateFishingPermitRequest): Promise<FishingPermitResponse> => {
    const response = await apiClient.post<FishingPermitResponse>('/api/fishingpermits', data);
    return response.data;
  },

  revoke: async (id: number): Promise<void> => {
    await apiClient.post(`/api/fishingpermits/${id}/revoke`);
// ==================== FISHING LOGS API ====================

export interface CatchCompositionRequest {
  fishSpecies: string;
  weightKg?: number;
  count?: number;
  status?: string;
}

export interface CreateFishingLogEntryRequest {
  shipId: number;
  logDate: string;
  startTime?: string;
  endTime?: string;
  fishingZone?: string;
  catchDetails?: string;
  routeDetails?: string;
  catchCompositions: CatchCompositionRequest[];
}

export interface CatchCompositionResponse {
  catchId: number;
  fishSpecies: string;
  weightKg?: number;
  count?: number;
  status?: string;
}

export interface FishingLogEntryResponse {
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
  catchCompositions: CatchCompositionResponse[];
} isSigned: boolean;
  signedOn?: string;
  createdOn: string;
}

export const fishingLogsApi = {
  getById: async (id: number): Promise<FishingLogEntryResponse> => {
    const response = await apiClient.get<FishingLogEntryResponse>(`/api/fishinglogs/${id}`);
    return response.data;
  },

  getByShipId: async (shipId: number): Promise<FishingLogEntryResponse[]> => {
// ==================== INSPECTIONS API ====================

export interface CreateInspectionRequest {
  shipId: number;
  inspectionLocation?: string;
  protocolNumber: string;
  hasViolation: boolean;
  violationDescription?: string;
  sanctionsImposed?: string;
  proofOfViolationUrl?: string;
}

export interface InspectionResponse {
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
export interface InspectionResponse {
  inspectionId: number;
  shipId: number;
  inspectorId: number;
  inspectionType: string;
  inspectionDate: string;
  location: string;
  notes: string;
  violationFound: boolean;
  violationDetails?: string;
  isProcessed: boolean;
  processedOn?: string;
  createdOn: string;
}

export const inspectionsApi = {
  getById: async (id: number): Promise<InspectionResponse> => {
    const response = await apiClient.get<InspectionResponse>(`/api/inspections/${id}`);
    return response.data;
  },

  getByShipId: async (shipId: number): Promise<InspectionResponse[]> => {
    const response = await apiClient.get<InspectionResponse[]>(`/api/inspections/ship/${shipId}`);
    return response.data;
  },

  create: async (data: CreateInspectionRequest): Promise<InspectionResponse> => {
    const response = await apiClient.post<InspectionResponse>('/api/inspections', data);
    return response.data;
  },

  process: async (id: number): Promise<void> => {
    await apiClient.post(`/api/inspections/${id}/process`);
  },
};

// ==================== EXPORT ALL ====================

export default {
  auth: authApi,
  fishingShips: fishingShipsApi,
  fishingPermits: fishingPermitsApi,
  fishingLogs: fishingLogsApi,
  inspections: inspectionsApi,
};
