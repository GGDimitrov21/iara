using Iara.Application.Common;
using Iara.Application.DTOs.Inspections;
using Iara.Application.Services;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;

namespace Iara.Infrastructure.Services;

public class NewInspectionService : IInspectionService
{
    private readonly IInspectionRepository _inspectionRepository;
    private readonly IVesselRepository _vesselRepository;
    private readonly IPersonnelRepository _personnelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NewInspectionService(
        IInspectionRepository inspectionRepository,
        IVesselRepository vesselRepository,
        IPersonnelRepository personnelRepository,
        IUnitOfWork unitOfWork)
    {
        _inspectionRepository = inspectionRepository;
        _vesselRepository = vesselRepository;
        _personnelRepository = personnelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<InspectionDto>> GetByIdAsync(int inspectionId, CancellationToken cancellationToken = default)
    {
        var inspection = await _inspectionRepository.GetWithDetailsAsync(inspectionId, cancellationToken);
        if (inspection == null)
            return Result<InspectionDto>.Failure("Inspection not found");

        return Result<InspectionDto>.Success(MapToDto(inspection));
    }

    public async Task<Result<IEnumerable<InspectionDto>>> GetByVesselAsync(int vesselId, CancellationToken cancellationToken = default)
    {
        var inspections = await _inspectionRepository.GetByVesselIdAsync(vesselId, cancellationToken);
        return Result<IEnumerable<InspectionDto>>.Success(inspections.Select(MapToDto));
    }

    public async Task<Result<IEnumerable<InspectionDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var inspections = await _inspectionRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<InspectionDto>>.Success(inspections.Select(i => MapToDto(i)));
    }

    public async Task<Result<InspectionDto>> CreateAsync(CreateInspectionRequest request, CancellationToken cancellationToken = default)
    {
        var vessel = await _vesselRepository.GetByIdAsync(request.VesselId, cancellationToken);
        if (vessel == null)
            return Result<InspectionDto>.Failure("Vessel not found");

        var inspector = await _personnelRepository.GetByIdAsync(request.InspectorId, cancellationToken);
        if (inspector == null)
            return Result<InspectionDto>.Failure("Inspector not found");

        var inspection = new Inspection
        {
            VesselId = request.VesselId,
            InspectorId = request.InspectorId,
            InspectionDate = request.InspectionDate,
            IsLegal = request.IsLegal,
            Notes = request.Notes
        };

        await _inspectionRepository.AddAsync(inspection, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        inspection.Vessel = vessel;
        inspection.Inspector = inspector;

        return Result<InspectionDto>.Success(MapToDto(inspection));
    }

    public async Task<Result<InspectionDto>> UpdateAsync(int inspectionId, UpdateInspectionRequest request, CancellationToken cancellationToken = default)
    {
        var inspection = await _inspectionRepository.GetWithDetailsAsync(inspectionId, cancellationToken);
        if (inspection == null)
            return Result<InspectionDto>.Failure("Inspection not found");

        inspection.InspectionDate = request.InspectionDate;
        inspection.IsLegal = request.IsLegal;
        inspection.Notes = request.Notes;

        _inspectionRepository.Update(inspection);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<InspectionDto>.Success(MapToDto(inspection));
    }

    public async Task<Result> DeleteAsync(int inspectionId, CancellationToken cancellationToken = default)
    {
        var inspection = await _inspectionRepository.GetByIdAsync(inspectionId, cancellationToken);
        if (inspection == null)
            return Result.Failure("Inspection not found");

        _inspectionRepository.Remove(inspection);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static InspectionDto MapToDto(Inspection inspection)
    {
        return new InspectionDto
        {
            InspectionId = inspection.InspectionId,
            VesselId = inspection.VesselId,
            VesselName = inspection.Vessel?.VesselName ?? "Unknown",
            InspectorId = inspection.InspectorId,
            InspectorName = inspection.Inspector?.Name ?? "Unknown",
            InspectionDate = inspection.InspectionDate,
            IsLegal = inspection.IsLegal,
            Notes = inspection.Notes
        };
    }
}
