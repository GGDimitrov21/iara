using Iara.Application.Common;
using Iara.Application.DTOs.Vessels;
using Iara.Application.Services;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;

namespace Iara.Infrastructure.Services;

public class VesselService : IVesselService
{
    private readonly IVesselRepository _vesselRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VesselService(IVesselRepository vesselRepository, IUnitOfWork unitOfWork)
    {
        _vesselRepository = vesselRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<VesselDto>> GetByIdAsync(int vesselId, CancellationToken cancellationToken = default)
    {
        var vessel = await _vesselRepository.GetWithDetailsAsync(vesselId, cancellationToken);
        if (vessel == null)
            return Result<VesselDto>.Failure("Vessel not found");

        return Result<VesselDto>.Success(MapToDto(vessel));
    }

    public async Task<Result<IEnumerable<VesselDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var vessels = await _vesselRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<VesselDto>>.Success(vessels.Select(MapToDto));
    }

    public async Task<Result<VesselDto>> CreateAsync(CreateVesselRequest request, CancellationToken cancellationToken = default)
    {
        var existingVessel = await _vesselRepository.GetByRegNumberAsync(request.RegNumber, cancellationToken);
        if (existingVessel != null)
            return Result<VesselDto>.Failure("A vessel with this registration number already exists");

        var vessel = new Vessel
        {
            RegNumber = request.RegNumber,
            VesselName = request.VesselName,
            OwnerDetails = request.OwnerDetails,
            CaptainId = request.CaptainId,
            LengthM = request.LengthM,
            WidthM = request.WidthM,
            Tonnage = request.Tonnage,
            FuelType = request.FuelType,
            EnginePowerKw = request.EnginePowerKw,
            DisplacementTons = request.DisplacementTons
        };

        await _vesselRepository.AddAsync(vessel, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<VesselDto>.Success(MapToDto(vessel));
    }

    public async Task<Result<VesselDto>> UpdateAsync(int vesselId, UpdateVesselRequest request, CancellationToken cancellationToken = default)
    {
        var vessel = await _vesselRepository.GetByIdAsync(vesselId, cancellationToken);
        if (vessel == null)
            return Result<VesselDto>.Failure("Vessel not found");

        vessel.VesselName = request.VesselName;
        vessel.OwnerDetails = request.OwnerDetails;
        vessel.CaptainId = request.CaptainId;
        vessel.LengthM = request.LengthM;
        vessel.WidthM = request.WidthM;
        vessel.Tonnage = request.Tonnage;
        vessel.FuelType = request.FuelType;
        vessel.EnginePowerKw = request.EnginePowerKw;
        vessel.DisplacementTons = request.DisplacementTons;

        _vesselRepository.Update(vessel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<VesselDto>.Success(MapToDto(vessel));
    }

    public async Task<Result> DeleteAsync(int vesselId, CancellationToken cancellationToken = default)
    {
        var vessel = await _vesselRepository.GetByIdAsync(vesselId, cancellationToken);
        if (vessel == null)
            return Result.Failure("Vessel not found");

        _vesselRepository.Remove(vessel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static VesselDto MapToDto(Vessel vessel)
    {
        return new VesselDto
        {
            VesselId = vessel.VesselId,
            RegNumber = vessel.RegNumber,
            VesselName = vessel.VesselName,
            OwnerDetails = vessel.OwnerDetails,
            CaptainId = vessel.CaptainId,
            CaptainName = vessel.Captain?.Name,
            LengthM = vessel.LengthM,
            WidthM = vessel.WidthM,
            Tonnage = vessel.Tonnage,
            FuelType = vessel.FuelType,
            EnginePowerKw = vessel.EnginePowerKw,
            DisplacementTons = vessel.DisplacementTons
        };
    }
}
