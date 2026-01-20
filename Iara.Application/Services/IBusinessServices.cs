using Iara.Application.Common;
using Iara.Application.DTOs.Vessels;
using Iara.Application.DTOs.Permits;
using Iara.Application.DTOs.Logbook;
using Iara.Application.DTOs.Inspections;
using Iara.Application.DTOs.Species;
using Iara.Application.DTOs.CatchQuotas;

namespace Iara.Application.Services;

public interface IVesselService
{
    Task<Result<VesselDto>> GetByIdAsync(int vesselId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<VesselDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<VesselDto>> CreateAsync(CreateVesselRequest request, CancellationToken cancellationToken = default);
    Task<Result<VesselDto>> UpdateAsync(int vesselId, UpdateVesselRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int vesselId, CancellationToken cancellationToken = default);
}

public interface IPermitService
{
    Task<Result<PermitDto>> GetByIdAsync(int permitId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PermitDto>>> GetByVesselAsync(int vesselId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PermitDto>>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<Result<PermitDto>> CreateAsync(CreatePermitRequest request, CancellationToken cancellationToken = default);
    Task<Result> RevokePermitAsync(int permitId, CancellationToken cancellationToken = default);
}

public interface ILogbookService
{
    Task<Result<LogbookEntryDto>> GetByIdAsync(int logEntryId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<LogbookEntryDto>>> GetByVesselAsync(int vesselId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<LogbookEntryDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<LogbookEntryDto>> CreateAsync(CreateLogbookEntryRequest request, CancellationToken cancellationToken = default);
    Task<Result<LogbookEntryDto>> UpdateAsync(int logEntryId, UpdateLogbookEntryRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int logEntryId, CancellationToken cancellationToken = default);
}

public interface IInspectionService
{
    Task<Result<InspectionDto>> GetByIdAsync(int inspectionId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<InspectionDto>>> GetByVesselAsync(int vesselId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<InspectionDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<InspectionDto>> CreateAsync(CreateInspectionRequest request, CancellationToken cancellationToken = default);
    Task<Result<InspectionDto>> UpdateAsync(int inspectionId, UpdateInspectionRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int inspectionId, CancellationToken cancellationToken = default);
}

public interface ISpeciesService
{
    Task<Result<SpeciesDto>> GetByIdAsync(int speciesId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<SpeciesDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<SpeciesDto>> CreateAsync(CreateSpeciesRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int speciesId, CancellationToken cancellationToken = default);
}

public interface ICatchQuotaService
{
    Task<Result<CatchQuotaDto>> GetByIdAsync(int quotaId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CatchQuotaDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CatchQuotaDto>>> GetByPermitAsync(int permitId, CancellationToken cancellationToken = default);
    Task<Result<CatchQuotaDto>> CreateAsync(CreateCatchQuotaRequest request, CancellationToken cancellationToken = default);
    Task<Result<CatchQuotaDto>> UpdateAsync(int quotaId, UpdateCatchQuotaRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int quotaId, CancellationToken cancellationToken = default);
}
