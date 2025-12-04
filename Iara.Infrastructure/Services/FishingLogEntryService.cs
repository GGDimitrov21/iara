using Iara.Application.Common;
using Iara.Application.DTOs.FishingLogs;
using Iara.Application.Services;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Iara.Infrastructure.Services;

public class FishingLogEntryService : IFishingLogEntryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FishingLogEntryService> _logger;

    public FishingLogEntryService(IUnitOfWork unitOfWork, ILogger<FishingLogEntryService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<FishingLogEntryDto>> GetByIdAsync(long logEntryId, CancellationToken cancellationToken = default)
    {
        var logEntry = await _unitOfWork.FishingLogEntries.GetLogEntryWithCatchesAsync(logEntryId, cancellationToken);
        
        if (logEntry == null)
            return Result<FishingLogEntryDto>.Failure($"Log entry with ID {logEntryId} not found");

        var dto = MapToDto(logEntry);
        return Result<FishingLogEntryDto>.Success(dto);
    }

    public async Task<Result<IEnumerable<FishingLogEntryDto>>> GetLogsByShipAsync(int shipId, CancellationToken cancellationToken = default)
    {
        var logs = await _unitOfWork.FishingLogEntries.GetLogEntriesByShipIdAsync(shipId, cancellationToken);
        var dtos = logs.Select(MapToDto);
        return Result<IEnumerable<FishingLogEntryDto>>.Success(dtos);
    }

    public async Task<Result<FishingLogEntryDto>> CreateAsync(CreateFishingLogEntryRequest request, CancellationToken cancellationToken = default)
    {
        // Verify ship exists
        var ship = await _unitOfWork.FishingShips.GetByIdAsync(request.ShipId, cancellationToken);
        if (ship == null)
            return Result<FishingLogEntryDto>.Failure($"Ship with ID {request.ShipId} not found");

        // Check for duplicate log entry for same date
        if (await _unitOfWork.FishingLogEntries.LogEntryExistsForDateAsync(request.ShipId, request.LogDate, cancellationToken))
            return Result<FishingLogEntryDto>.Failure($"Log entry already exists for ship {request.ShipId} on {request.LogDate}");

        var logEntry = new FishingLogEntry
        {
            ShipId = request.ShipId,
            LogDate = request.LogDate,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            FishingZone = request.FishingZone,
            CatchDetails = request.CatchDetails,
            RouteDetails = request.RouteDetails,
            IsSigned = false,
            SubmittedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.FishingLogEntries.AddAsync(logEntry, cancellationToken);
        
        // Add catch compositions
        foreach (var catchRequest in request.CatchCompositions)
        {
            var catchComposition = new CatchComposition
            {
                LogEntryId = logEntry.LogEntryId,
                FishSpecies = catchRequest.FishSpecies,
                WeightKg = catchRequest.WeightKg,
                Count = catchRequest.Count,
                Status = catchRequest.Status,
                CreatedAt = DateTime.UtcNow
            };
            
            await _unitOfWork.CatchCompositions.AddAsync(catchComposition, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created fishing log entry {LogEntryId} for ship {ShipId}", logEntry.LogEntryId, logEntry.ShipId);

        // Reload with catches
        var createdEntry = await _unitOfWork.FishingLogEntries.GetLogEntryWithCatchesAsync(logEntry.LogEntryId, cancellationToken);
        var dto = MapToDto(createdEntry!);
        return Result<FishingLogEntryDto>.Success(dto);
    }

    public async Task<Result> SignLogEntryAsync(long logEntryId, CancellationToken cancellationToken = default)
    {
        var logEntry = await _unitOfWork.FishingLogEntries.GetByIdAsync(logEntryId, cancellationToken);
        
        if (logEntry == null)
            return Result.Failure($"Log entry with ID {logEntryId} not found");

        if (logEntry.IsSigned)
            return Result.Failure("Log entry is already signed");

        logEntry.IsSigned = true;
        logEntry.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.FishingLogEntries.Update(logEntry);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Signed log entry {LogEntryId}", logEntryId);

        return Result.Success();
    }

    private static FishingLogEntryDto MapToDto(FishingLogEntry logEntry)
    {
        return new FishingLogEntryDto
        {
            LogEntryId = logEntry.LogEntryId,
            ShipId = logEntry.ShipId,
            ShipName = logEntry.Ship?.ShipName ?? "Unknown",
            LogDate = logEntry.LogDate,
            StartTime = logEntry.StartTime,
            EndTime = logEntry.EndTime,
            FishingZone = logEntry.FishingZone,
            CatchDetails = logEntry.CatchDetails,
            RouteDetails = logEntry.RouteDetails,
            IsSigned = logEntry.IsSigned,
            SubmittedAt = logEntry.SubmittedAt,
            CatchCompositions = logEntry.CatchCompositions.Select(c => new CatchCompositionDto
            {
                CatchId = c.CatchId,
                FishSpecies = c.FishSpecies,
                WeightKg = c.WeightKg,
                Count = c.Count,
                Status = c.Status
            }).ToList()
        };
    }
}
