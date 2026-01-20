using Iara.Application.Common;
using Iara.Application.DTOs.CatchQuotas;
using Iara.Application.Services;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;

namespace Iara.Infrastructure.Services;

public class CatchQuotaService : ICatchQuotaService
{
    private readonly ICatchQuotaRepository _quotaRepository;
    private readonly IPermitRepository _permitRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CatchQuotaService(
        ICatchQuotaRepository quotaRepository,
        IPermitRepository permitRepository,
        ISpeciesRepository speciesRepository,
        IUnitOfWork unitOfWork)
    {
        _quotaRepository = quotaRepository;
        _permitRepository = permitRepository;
        _speciesRepository = speciesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CatchQuotaDto>> GetByIdAsync(int quotaId, CancellationToken cancellationToken = default)
    {
        var quota = await _quotaRepository.GetByIdAsync(quotaId, cancellationToken);
        if (quota == null)
            return Result<CatchQuotaDto>.Failure("Catch quota not found");

        var species = await _speciesRepository.GetByIdAsync(quota.SpeciesId, cancellationToken);
        return Result<CatchQuotaDto>.Success(MapToDto(quota, species?.SpeciesName ?? "Unknown"));
    }

    public async Task<Result<IEnumerable<CatchQuotaDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var quotas = await _quotaRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<CatchQuotaDto>>.Success(quotas.Select(q => MapToDto(q, q.Species?.SpeciesName ?? "Unknown")));
    }

    public async Task<Result<IEnumerable<CatchQuotaDto>>> GetByPermitAsync(int permitId, CancellationToken cancellationToken = default)
    {
        var quotas = await _quotaRepository.GetByPermitIdAsync(permitId, cancellationToken);
        return Result<IEnumerable<CatchQuotaDto>>.Success(quotas.Select(q => MapToDto(q, q.Species?.SpeciesName ?? "Unknown")));
    }

    public async Task<Result<CatchQuotaDto>> CreateAsync(CreateCatchQuotaRequest request, CancellationToken cancellationToken = default)
    {
        var permit = await _permitRepository.GetByIdAsync(request.PermitId, cancellationToken);
        if (permit == null)
            return Result<CatchQuotaDto>.Failure("Permit not found");

        var species = await _speciesRepository.GetByIdAsync(request.SpeciesId, cancellationToken);
        if (species == null)
            return Result<CatchQuotaDto>.Failure("Species not found");

        var existingQuota = await _quotaRepository.GetByPermitSpeciesYearAsync(request.PermitId, request.SpeciesId, request.Year, cancellationToken);
        if (existingQuota != null)
            return Result<CatchQuotaDto>.Failure("A quota for this permit/species/year already exists");

        var quota = new CatchQuota
        {
            PermitId = request.PermitId,
            SpeciesId = request.SpeciesId,
            Year = request.Year,
            MinCatchKg = request.MinCatchKg,
            AvgCatchKg = request.AvgCatchKg,
            MaxCatchKg = request.MaxCatchKg,
            FuelHoursLimit = request.FuelHoursLimit
        };

        await _quotaRepository.AddAsync(quota, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CatchQuotaDto>.Success(MapToDto(quota, species.SpeciesName));
    }

    public async Task<Result<CatchQuotaDto>> UpdateAsync(int quotaId, UpdateCatchQuotaRequest request, CancellationToken cancellationToken = default)
    {
        var quota = await _quotaRepository.GetByIdAsync(quotaId, cancellationToken);
        if (quota == null)
            return Result<CatchQuotaDto>.Failure("Catch quota not found");

        quota.MinCatchKg = request.MinCatchKg;
        quota.AvgCatchKg = request.AvgCatchKg;
        quota.MaxCatchKg = request.MaxCatchKg;
        quota.FuelHoursLimit = request.FuelHoursLimit;

        _quotaRepository.Update(quota);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var species = await _speciesRepository.GetByIdAsync(quota.SpeciesId, cancellationToken);
        return Result<CatchQuotaDto>.Success(MapToDto(quota, species?.SpeciesName ?? "Unknown"));
    }

    public async Task<Result> DeleteAsync(int quotaId, CancellationToken cancellationToken = default)
    {
        var quota = await _quotaRepository.GetByIdAsync(quotaId, cancellationToken);
        if (quota == null)
            return Result.Failure("Catch quota not found");

        _quotaRepository.Remove(quota);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static CatchQuotaDto MapToDto(CatchQuota quota, string speciesName)
    {
        return new CatchQuotaDto
        {
            QuotaId = quota.QuotaId,
            PermitId = quota.PermitId,
            SpeciesId = quota.SpeciesId,
            SpeciesName = speciesName,
            Year = quota.Year,
            MinCatchKg = quota.MinCatchKg,
            AvgCatchKg = quota.AvgCatchKg,
            MaxCatchKg = quota.MaxCatchKg,
            FuelHoursLimit = quota.FuelHoursLimit
        };
    }
}
