using Iara.Application.Common;
using Iara.Application.DTOs.Logbook;
using Iara.Application.Services;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;

namespace Iara.Infrastructure.Services;

public class LogbookService : ILogbookService
{
    private readonly ILogbookRepository _logbookRepository;
    private readonly IVesselRepository _vesselRepository;
    private readonly IPersonnelRepository _personnelRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogbookService(
        ILogbookRepository logbookRepository,
        IVesselRepository vesselRepository,
        IPersonnelRepository personnelRepository,
        ISpeciesRepository speciesRepository,
        IUnitOfWork unitOfWork)
    {
        _logbookRepository = logbookRepository;
        _vesselRepository = vesselRepository;
        _personnelRepository = personnelRepository;
        _speciesRepository = speciesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LogbookEntryDto>> GetByIdAsync(int logEntryId, CancellationToken cancellationToken = default)
    {
        var entry = await _logbookRepository.GetWithDetailsAsync(logEntryId, cancellationToken);
        if (entry == null)
            return Result<LogbookEntryDto>.Failure("Logbook entry not found");

        return Result<LogbookEntryDto>.Success(MapToDto(entry));
    }

    public async Task<Result<IEnumerable<LogbookEntryDto>>> GetByVesselAsync(int vesselId, CancellationToken cancellationToken = default)
    {
        var entries = await _logbookRepository.GetByVesselIdAsync(vesselId, cancellationToken);
        return Result<IEnumerable<LogbookEntryDto>>.Success(entries.Select(MapToDto));
    }

    public async Task<Result<IEnumerable<LogbookEntryDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entries = await _logbookRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<LogbookEntryDto>>.Success(entries.Select(e => MapToDto(e)));
    }

    public async Task<Result<LogbookEntryDto>> CreateAsync(CreateLogbookEntryRequest request, CancellationToken cancellationToken = default)
    {
        var vessel = await _vesselRepository.GetByIdAsync(request.VesselId, cancellationToken);
        if (vessel == null)
            return Result<LogbookEntryDto>.Failure("Vessel not found");

        var captain = await _personnelRepository.GetByIdAsync(request.CaptainId, cancellationToken);
        if (captain == null)
            return Result<LogbookEntryDto>.Failure("Captain not found");

        var species = await _speciesRepository.GetByIdAsync(request.SpeciesId, cancellationToken);
        if (species == null)
            return Result<LogbookEntryDto>.Failure("Species not found");

        var entry = new Logbook
        {
            VesselId = request.VesselId,
            CaptainId = request.CaptainId,
            StartTime = request.StartTime,
            DurationHours = request.DurationHours,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            SpeciesId = request.SpeciesId,
            CatchKg = request.CatchKg
        };

        await _logbookRepository.AddAsync(entry, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        entry.Vessel = vessel;
        entry.Captain = captain;
        entry.Species = species;

        return Result<LogbookEntryDto>.Success(MapToDto(entry));
    }

    public async Task<Result<LogbookEntryDto>> UpdateAsync(int logEntryId, UpdateLogbookEntryRequest request, CancellationToken cancellationToken = default)
    {
        var entry = await _logbookRepository.GetWithDetailsAsync(logEntryId, cancellationToken);
        if (entry == null)
            return Result<LogbookEntryDto>.Failure("Logbook entry not found");

        var species = await _speciesRepository.GetByIdAsync(request.SpeciesId, cancellationToken);
        if (species == null)
            return Result<LogbookEntryDto>.Failure("Species not found");

        entry.StartTime = request.StartTime;
        entry.DurationHours = request.DurationHours;
        entry.Latitude = request.Latitude;
        entry.Longitude = request.Longitude;
        entry.SpeciesId = request.SpeciesId;
        entry.CatchKg = request.CatchKg;

        _logbookRepository.Update(entry);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        entry.Species = species;
        return Result<LogbookEntryDto>.Success(MapToDto(entry));
    }

    public async Task<Result> DeleteAsync(int logEntryId, CancellationToken cancellationToken = default)
    {
        var entry = await _logbookRepository.GetByIdAsync(logEntryId, cancellationToken);
        if (entry == null)
            return Result.Failure("Logbook entry not found");

        _logbookRepository.Remove(entry);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static LogbookEntryDto MapToDto(Logbook entry)
    {
        return new LogbookEntryDto
        {
            LogEntryId = entry.LogEntryId,
            VesselId = entry.VesselId,
            VesselName = entry.Vessel?.VesselName ?? "Unknown",
            CaptainId = entry.CaptainId,
            CaptainName = entry.Captain?.Name ?? "Unknown",
            StartTime = entry.StartTime,
            DurationHours = entry.DurationHours,
            Latitude = entry.Latitude,
            Longitude = entry.Longitude,
            SpeciesId = entry.SpeciesId,
            SpeciesName = entry.Species?.SpeciesName ?? "Unknown",
            CatchKg = entry.CatchKg
        };
    }
}
