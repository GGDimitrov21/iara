using Iara.Application.Common;
using Iara.Application.DTOs.FishingShips;
using Iara.Application.DTOs.FishingPermits;
using Iara.Application.DTOs.FishingLogs;
using Iara.Application.DTOs.Inspections;

namespace Iara.Application.Services;

public interface IFishingShipService
{
    Task<Result<FishingShipDto>> GetByIdAsync(int shipId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<FishingShipDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<FishingShipDto>> CreateAsync(CreateFishingShipRequest request, CancellationToken cancellationToken = default);
    Task<Result<FishingShipDto>> UpdateAsync(int shipId, UpdateFishingShipRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int shipId, CancellationToken cancellationToken = default);
}

public interface IFishingPermitService
{
    Task<Result<FishingPermitDto>> GetByIdAsync(int permitId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<FishingPermitDto>>> GetPermitsByShipAsync(int shipId, CancellationToken cancellationToken = default);
    Task<Result<FishingPermitDto>> CreateAsync(CreateFishingPermitRequest request, CancellationToken cancellationToken = default);
    Task<Result> RevokePermitAsync(int permitId, CancellationToken cancellationToken = default);
}

public interface IFishingLogEntryService
{
    Task<Result<FishingLogEntryDto>> GetByIdAsync(long logEntryId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<FishingLogEntryDto>>> GetLogsByShipAsync(int shipId, CancellationToken cancellationToken = default);
    Task<Result<FishingLogEntryDto>> CreateAsync(CreateFishingLogEntryRequest request, CancellationToken cancellationToken = default);
    Task<Result> SignLogEntryAsync(long logEntryId, CancellationToken cancellationToken = default);
}

public interface IInspectionService
{
    Task<Result<InspectionDto>> GetByIdAsync(int inspectionId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<InspectionDto>>> GetInspectionsByShipAsync(int shipId, CancellationToken cancellationToken = default);
    Task<Result<InspectionDto>> CreateAsync(CreateInspectionRequest request, int inspectorId, CancellationToken cancellationToken = default);
    Task<Result> ProcessInspectionAsync(int inspectionId, CancellationToken cancellationToken = default);
}
