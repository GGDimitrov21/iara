using Iara.Application.Common;
using Iara.Application.DTOs.Permits;
using Iara.Application.Services;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;

namespace Iara.Infrastructure.Services;

public class PermitService : IPermitService
{
    private readonly IPermitRepository _permitRepository;
    private readonly IVesselRepository _vesselRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PermitService(
        IPermitRepository permitRepository, 
        IVesselRepository vesselRepository,
        IUnitOfWork unitOfWork)
    {
        _permitRepository = permitRepository;
        _vesselRepository = vesselRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PermitDto>> GetByIdAsync(int permitId, CancellationToken cancellationToken = default)
    {
        var permit = await _permitRepository.GetByIdAsync(permitId, cancellationToken);
        if (permit == null)
            return Result<PermitDto>.Failure("Permit not found");

        var vessel = await _vesselRepository.GetByIdAsync(permit.VesselId, cancellationToken);
        return Result<PermitDto>.Success(MapToDto(permit, vessel?.VesselName ?? "Unknown"));
    }

    public async Task<Result<IEnumerable<PermitDto>>> GetByVesselAsync(int vesselId, CancellationToken cancellationToken = default)
    {
        var permits = await _permitRepository.GetByVesselIdAsync(vesselId, cancellationToken);
        return Result<IEnumerable<PermitDto>>.Success(permits.Select(p => MapToDto(p, p.Vessel?.VesselName ?? "Unknown")));
    }

    public async Task<Result<IEnumerable<PermitDto>>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        var permits = await _permitRepository.GetActivePermitsAsync(cancellationToken);
        return Result<IEnumerable<PermitDto>>.Success(permits.Select(p => MapToDto(p, p.Vessel?.VesselName ?? "Unknown")));
    }

    public async Task<Result<PermitDto>> CreateAsync(CreatePermitRequest request, CancellationToken cancellationToken = default)
    {
        var vessel = await _vesselRepository.GetByIdAsync(request.VesselId, cancellationToken);
        if (vessel == null)
            return Result<PermitDto>.Failure("Vessel not found");

        if (request.ExpiryDate <= request.IssueDate)
            return Result<PermitDto>.Failure("Expiry date must be after issue date");

        var permit = new Permit
        {
            VesselId = request.VesselId,
            IssueDate = request.IssueDate,
            ExpiryDate = request.ExpiryDate,
            IsActive = true
        };

        await _permitRepository.AddAsync(permit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<PermitDto>.Success(MapToDto(permit, vessel.VesselName));
    }

    public async Task<Result> RevokePermitAsync(int permitId, CancellationToken cancellationToken = default)
    {
        var permit = await _permitRepository.GetByIdAsync(permitId, cancellationToken);
        if (permit == null)
            return Result.Failure("Permit not found");

        permit.IsActive = false;
        _permitRepository.Update(permit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static PermitDto MapToDto(Permit permit, string vesselName)
    {
        return new PermitDto
        {
            PermitId = permit.PermitId,
            VesselId = permit.VesselId,
            VesselName = vesselName,
            IssueDate = permit.IssueDate,
            ExpiryDate = permit.ExpiryDate,
            IsActive = permit.IsActive
        };
    }
}
