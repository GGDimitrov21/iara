using Iara.Application.Common;
using Iara.Application.DTOs.FishingPermits;
using Iara.Application.Services;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Iara.Infrastructure.Services;

public class FishingPermitService : IFishingPermitService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FishingPermitService> _logger;

    public FishingPermitService(IUnitOfWork unitOfWork, ILogger<FishingPermitService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<FishingPermitDto>> GetByIdAsync(int permitId, CancellationToken cancellationToken = default)
    {
        var permit = await _unitOfWork.FishingPermits.GetByIdAsync(permitId, cancellationToken);
        
        if (permit == null)
            return Result<FishingPermitDto>.Failure($"Permit with ID {permitId} not found");

        var dto = await MapToDtoAsync(permit, cancellationToken);
        return Result<FishingPermitDto>.Success(dto);
    }

    public async Task<Result<IEnumerable<FishingPermitDto>>> GetPermitsByShipAsync(int shipId, CancellationToken cancellationToken = default)
    {
        var permits = await _unitOfWork.FishingPermits.GetPermitsByShipIdAsync(shipId, cancellationToken);
        var dtos = new List<FishingPermitDto>();
        
        foreach (var permit in permits)
        {
            dtos.Add(await MapToDtoAsync(permit, cancellationToken));
        }
        
        return Result<IEnumerable<FishingPermitDto>>.Success(dtos);
    }

    public async Task<Result<FishingPermitDto>> CreateAsync(CreateFishingPermitRequest request, CancellationToken cancellationToken = default)
    {
        // Verify ship exists
        var ship = await _unitOfWork.FishingShips.GetByIdAsync(request.ShipId, cancellationToken);
        if (ship == null)
            return Result<FishingPermitDto>.Failure($"Ship with ID {request.ShipId} not found");

        // Check for active permit for same year
        var existingPermit = await _unitOfWork.FishingPermits.GetActivePermitForShipAsync(request.ShipId, request.PermitYear, cancellationToken);
        if (existingPermit != null)
            return Result<FishingPermitDto>.Failure($"Active permit already exists for ship {request.ShipId} in year {request.PermitYear}");

        var permit = new FishingPermit
        {
            ShipId = request.ShipId,
            PermitYear = request.PermitYear,
            IssueDate = DateOnly.FromDateTime(DateTime.UtcNow),
            ValidFrom = request.ValidFrom,
            ValidUntil = request.ValidUntil,
            CatchQuotaType = request.CatchQuotaType,
            MinAnnualCatch = request.MinAnnualCatch,
            MaxAnnualCatch = request.MaxAnnualCatch,
            TotalHoursAnnualLimit = request.TotalHoursAnnualLimit,
            Status = PermitStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.FishingPermits.AddAsync(permit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created permit {PermitId} for ship {ShipId}", permit.PermitId, permit.ShipId);

        var dto = await MapToDtoAsync(permit, cancellationToken);
        return Result<FishingPermitDto>.Success(dto);
    }

    public async Task<Result> RevokePermitAsync(int permitId, CancellationToken cancellationToken = default)
    {
        var permit = await _unitOfWork.FishingPermits.GetByIdAsync(permitId, cancellationToken);
        
        if (permit == null)
            return Result.Failure($"Permit with ID {permitId} not found");

        permit.Status = PermitStatus.Revoked;
        permit.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.FishingPermits.Update(permit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogWarning("Revoked permit {PermitId}", permitId);

        return Result.Success();
    }

    private async Task<FishingPermitDto> MapToDtoAsync(FishingPermit permit, CancellationToken cancellationToken)
    {
        var ship = await _unitOfWork.FishingShips.GetByIdAsync(permit.ShipId, cancellationToken);
        
        return new FishingPermitDto
        {
            PermitId = permit.PermitId,
            ShipId = permit.ShipId,
            ShipName = ship?.ShipName ?? "Unknown",
            PermitYear = permit.PermitYear,
            IssueDate = permit.IssueDate,
            ValidFrom = permit.ValidFrom,
            ValidUntil = permit.ValidUntil,
            CatchQuotaType = permit.CatchQuotaType,
            MinAnnualCatch = permit.MinAnnualCatch,
            MaxAnnualCatch = permit.MaxAnnualCatch,
            TotalHoursAnnualLimit = permit.TotalHoursAnnualLimit,
            Status = permit.Status
        };
    }
}
