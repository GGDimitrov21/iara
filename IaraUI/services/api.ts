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

// ==================== PERSONNEL API ====================

export interface PersonnelDto {
  personId: number;
  name: string;
  role: string;
  contactEmail?: string;
}

export interface CreatePersonnelRequest {
  name: string;
  role: string;
  contactEmail?: string;
}

export const personnelApi = {
  getAll: async (): Promise<PersonnelDto[]> => {
    const response = await apiClient.get<PersonnelDto[]>('/api/personnel');
    return response.data;
  },

  getById: async (id: number): Promise<PersonnelDto> => {
    const response = await apiClient.get<PersonnelDto>(`/api/personnel/${id}`);
    return response.data;
  },

  getByRole: async (role: string): Promise<PersonnelDto[]> => {
    const response = await apiClient.get<PersonnelDto[]>(`/api/personnel/role/${role}`);
    return response.data;
  },

  create: async (data: CreatePersonnelRequest): Promise<PersonnelDto> => {
    const response = await apiClient.post<PersonnelDto>('/api/personnel', data);
    return response.data;
  },

  update: async (id: number, data: CreatePersonnelRequest): Promise<PersonnelDto> => {
    const response = await apiClient.put<PersonnelDto>(`/api/personnel/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/api/personnel/${id}`);
  },
};

// ==================== VESSELS API ====================

export interface VesselDto {
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

export interface CreateVesselRequest {
  regNumber: string;
  vesselName: string;
  ownerDetails?: string;
  captainId?: number;
  lengthM?: number;
  widthM?: number;
  tonnage?: number;
  fuelType?: string;
  enginePowerKw?: number;
  displacementTons?: number;
}

export interface UpdateVesselRequest {
  vesselName: string;
  ownerDetails?: string;
  captainId?: number;
  lengthM?: number;
  widthM?: number;
  tonnage?: number;
  fuelType?: string;
  enginePowerKw?: number;
  displacementTons?: number;
}

export const vesselsApi = {
  getAll: async (): Promise<VesselDto[]> => {
    const response = await apiClient.get<VesselDto[]>('/api/vessels');
    return response.data;
  },

  getById: async (id: number): Promise<VesselDto> => {
    const response = await apiClient.get<VesselDto>(`/api/vessels/${id}`);
    return response.data;
  },

  create: async (data: CreateVesselRequest): Promise<VesselDto> => {
    const response = await apiClient.post<VesselDto>('/api/vessels', data);
    return response.data;
  },

  update: async (id: number, data: UpdateVesselRequest): Promise<VesselDto> => {
    const response = await apiClient.put<VesselDto>(`/api/vessels/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/api/vessels/${id}`);
  },
};

// ==================== PERMITS API ====================

export interface PermitDto {
  permitId: number;
  vesselId: number;
  vesselName: string;
  issueDate: string;
  expiryDate: string;
  isActive: boolean;
}

export interface CreatePermitRequest {
  vesselId: number;
  issueDate: string;
  expiryDate: string;
}

export const permitsApi = {
  getAll: async (): Promise<PermitDto[]> => {
    const response = await apiClient.get<PermitDto[]>('/api/permits');
    return response.data;
  },

  getById: async (id: number): Promise<PermitDto> => {
    const response = await apiClient.get<PermitDto>(`/api/permits/${id}`);
    return response.data;
  },

  getByVesselId: async (vesselId: number): Promise<PermitDto[]> => {
    const response = await apiClient.get<PermitDto[]>(`/api/permits/vessel/${vesselId}`);
    return response.data;
  },

  create: async (data: CreatePermitRequest): Promise<PermitDto> => {
    const response = await apiClient.post<PermitDto>('/api/permits', data);
    return response.data;
  },

  revoke: async (id: number): Promise<void> => {
    await apiClient.post(`/api/permits/${id}/revoke`);
  },
};

// ==================== SPECIES API ====================

export interface SpeciesDto {
  speciesId: number;
  speciesName: string;
}

export interface CreateSpeciesRequest {
  speciesName: string;
}

export const speciesApi = {
  getAll: async (): Promise<SpeciesDto[]> => {
    const response = await apiClient.get<SpeciesDto[]>('/api/species');
    return response.data;
  },

  getById: async (id: number): Promise<SpeciesDto> => {
    const response = await apiClient.get<SpeciesDto>(`/api/species/${id}`);
    return response.data;
  },

  create: async (data: CreateSpeciesRequest): Promise<SpeciesDto> => {
    const response = await apiClient.post<SpeciesDto>('/api/species', data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/api/species/${id}`);
  },
};

// ==================== CATCH QUOTAS API ====================

export interface CatchQuotaDto {
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

export interface CreateCatchQuotaRequest {
  permitId: number;
  speciesId: number;
  year: number;
  minCatchKg?: number;
  avgCatchKg?: number;
  maxCatchKg: number;
  fuelHoursLimit?: number;
}

export interface UpdateCatchQuotaRequest {
  minCatchKg?: number;
  avgCatchKg?: number;
  maxCatchKg: number;
  fuelHoursLimit?: number;
}

export const quotasApi = {
  getAll: async (): Promise<CatchQuotaDto[]> => {
    const response = await apiClient.get<CatchQuotaDto[]>('/api/quotas');
    return response.data;
  },

  getById: async (id: number): Promise<CatchQuotaDto> => {
    const response = await apiClient.get<CatchQuotaDto>(`/api/quotas/${id}`);
    return response.data;
  },

  getByPermitId: async (permitId: number): Promise<CatchQuotaDto[]> => {
    const response = await apiClient.get<CatchQuotaDto[]>(`/api/quotas/permit/${permitId}`);
    return response.data;
  },

  create: async (data: CreateCatchQuotaRequest): Promise<CatchQuotaDto> => {
    const response = await apiClient.post<CatchQuotaDto>('/api/quotas', data);
    return response.data;
  },

  update: async (id: number, data: UpdateCatchQuotaRequest): Promise<CatchQuotaDto> => {
    const response = await apiClient.put<CatchQuotaDto>(`/api/quotas/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/api/quotas/${id}`);
  },
};

// ==================== LOGBOOK API ====================

export interface LogbookEntryDto {
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

export interface CreateLogbookEntryRequest {
  vesselId: number;
  captainId: number;
  startTime: string;
  durationHours?: number;
  latitude?: number;
  longitude?: number;
  speciesId: number;
  catchKg: number;
}

export interface UpdateLogbookEntryRequest {
  startTime: string;
  durationHours?: number;
  latitude?: number;
  longitude?: number;
  speciesId: number;
  catchKg: number;
}

export const logbookApi = {
  getAll: async (): Promise<LogbookEntryDto[]> => {
    const response = await apiClient.get<LogbookEntryDto[]>('/api/logbook');
    return response.data;
  },

  getById: async (id: number): Promise<LogbookEntryDto> => {
    const response = await apiClient.get<LogbookEntryDto>(`/api/logbook/${id}`);
    return response.data;
  },

  getByVesselId: async (vesselId: number): Promise<LogbookEntryDto[]> => {
    const response = await apiClient.get<LogbookEntryDto[]>(`/api/logbook/vessel/${vesselId}`);
    return response.data;
  },

  create: async (data: CreateLogbookEntryRequest): Promise<LogbookEntryDto> => {
    const response = await apiClient.post<LogbookEntryDto>('/api/logbook', data);
    return response.data;
  },

  update: async (id: number, data: UpdateLogbookEntryRequest): Promise<LogbookEntryDto> => {
    const response = await apiClient.put<LogbookEntryDto>(`/api/logbook/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/api/logbook/${id}`);
  },
};

// ==================== INSPECTIONS API ====================

export interface InspectionDto {
  inspectionId: number;
  vesselId: number;
  vesselName: string;
  inspectorId: number;
  inspectorName: string;
  inspectionDate: string;
  isLegal: boolean;
  notes?: string;
}

export interface CreateInspectionRequest {
  vesselId: number;
  inspectorId: number;
  inspectionDate: string;
  isLegal: boolean;
  notes?: string;
}

export interface UpdateInspectionRequest {
  inspectionDate: string;
  isLegal: boolean;
  notes?: string;
}

export const inspectionsApi = {
  getAll: async (): Promise<InspectionDto[]> => {
    const response = await apiClient.get<InspectionDto[]>('/api/inspections');
    return response.data;
  },

  getById: async (id: number): Promise<InspectionDto> => {
    const response = await apiClient.get<InspectionDto>(`/api/inspections/${id}`);
    return response.data;
  },

  getByVesselId: async (vesselId: number): Promise<InspectionDto[]> => {
    const response = await apiClient.get<InspectionDto[]>(`/api/inspections/vessel/${vesselId}`);
    return response.data;
  },

  create: async (data: CreateInspectionRequest): Promise<InspectionDto> => {
    const response = await apiClient.post<InspectionDto>('/api/inspections', data);
    return response.data;
  },

  update: async (id: number, data: UpdateInspectionRequest): Promise<InspectionDto> => {
    const response = await apiClient.put<InspectionDto>(`/api/inspections/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/api/inspections/${id}`);
  },
};

// ==================== EXPORT ALL ====================

export default {
  auth: authApi,
  personnel: personnelApi,
  vessels: vesselsApi,
  permits: permitsApi,
  species: speciesApi,
  quotas: quotasApi,
  logbook: logbookApi,
  inspections: inspectionsApi,
};
