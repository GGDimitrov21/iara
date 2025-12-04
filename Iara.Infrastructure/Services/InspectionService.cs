using Iara.Application.Common;
using Iara.Application.DTOs.Inspections;
using Iara.Application.Services;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Iara.Infrastructure.Services;

public class InspectionService : IInspectionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<InspectionService> _logger;

    public InspectionService(IUnitOfWork unitOfWork, ILogger<InspectionService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<InspectionDto>> GetByIdAsync(int inspectionId, CancellationToken cancellationToken = default)
    {
        var inspection = await _unitOfWork.Inspections.GetByIdAsync(inspectionId, cancellationToken);
        
        if (inspection == null)
            return Result<InspectionDto>.Failure($"Inspection with ID {inspectionId} not found");

        var dto = MapToDto(inspection);
        return Result<InspectionDto>.Success(dto);
    }

    public async Task<Result<IEnumerable<InspectionDto>>> GetInspectionsByShipAsync(int shipId, CancellationToken cancellationToken = default)
    {
        var inspections = await _unitOfWork.Inspections.GetInspectionsByShipIdAsync(shipId, cancellationToken);
        var dtos = inspections.Select(MapToDto);
        return Result<IEnumerable<InspectionDto>>.Success(dtos);
    }

    public async Task<Result<InspectionDto>> CreateAsync(CreateInspectionRequest request, int inspectorId, CancellationToken cancellationToken = default)
    {
        // Verify ship exists
        var ship = await _unitOfWork.FishingShips.GetByIdAsync(request.ShipId, cancellationToken);
        if (ship == null)
            return Result<InspectionDto>.Failure($"Ship with ID {request.ShipId} not found");

        // Verify inspector exists
        var inspector = await _unitOfWork.Users.GetByIdAsync(inspectorId, cancellationToken);
        if (inspector == null)
            return Result<InspectionDto>.Failure($"Inspector with ID {inspectorId} not found");

        // Check for duplicate protocol number
        if (await _unitOfWork.Inspections.AnyAsync(i => i.ProtocolNumber == request.ProtocolNumber, cancellationToken))
            return Result<InspectionDto>.Failure($"Inspection with protocol number {request.ProtocolNumber} already exists");

        var inspection = new Inspection
        {
            InspectorId = inspectorId,
            ShipId = request.ShipId,
            InspectionDate = DateTime.UtcNow,
            InspectionLocation = request.InspectionLocation,
            ProtocolNumber = request.ProtocolNumber,
            HasViolation = request.HasViolation,
            ViolationDescription = request.ViolationDescription,
            SanctionsImposed = request.SanctionsImposed,
            ProofOfViolationUrl = request.ProofOfViolationUrl,
            IsProcessed = false,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Inspections.AddAsync(inspection, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created inspection {InspectionId} by inspector {InspectorId} for ship {ShipId}", 
            inspection.InspectionId, inspectorId, request.ShipId);

        if (request.HasViolation)
        {
            _logger.LogWarning("Violation detected in inspection {InspectionId}: {Description}", 
                inspection.InspectionId, request.ViolationDescription);
        }

        // Reload with navigation properties
        var createdInspection = await _unitOfWork.Inspections.GetByIdAsync(inspection.InspectionId, cancellationToken);
        var dto = MapToDto(createdInspection!);
        return Result<InspectionDto>.Success(dto);
    }

    public async Task<Result> ProcessInspectionAsync(int inspectionId, CancellationToken cancellationToken = default)
    {
        var inspection = await _unitOfWork.Inspections.GetByIdAsync(inspectionId, cancellationToken);
        
        if (inspection == null)
            return Result.Failure($"Inspection with ID {inspectionId} not found");

        if (inspection.IsProcessed)
            return Result.Failure("Inspection is already processed");

        inspection.IsProcessed = true;
        inspection.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Inspections.Update(inspection);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Processed inspection {InspectionId}", inspectionId);

        return Result.Success();
    }

    private static InspectionDto MapToDto(Inspection inspection)
    {
        return new InspectionDto
        {
            InspectionId = inspection.InspectionId,
            InspectorId = inspection.InspectorId,
            InspectorName = inspection.Inspector?.FullName,
            ShipId = inspection.ShipId,
            ShipName = inspection.Ship?.ShipName ?? "Unknown",
            InspectionDate = inspection.InspectionDate,
            InspectionLocation = inspection.InspectionLocation,
            ProtocolNumber = inspection.ProtocolNumber,
            HasViolation = inspection.HasViolation,
            ViolationDescription = inspection.ViolationDescription,
            SanctionsImposed = inspection.SanctionsImposed,
            ProofOfViolationUrl = inspection.ProofOfViolationUrl,
            IsProcessed = inspection.IsProcessed
        };
    }
}
