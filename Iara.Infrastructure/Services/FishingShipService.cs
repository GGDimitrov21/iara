using Iara.Application.Common;
using Iara.Application.DTOs.FishingShips;
using Iara.Application.Services;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Iara.Infrastructure.Services;

public class FishingShipService : IFishingShipService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FishingShipService> _logger;

    public FishingShipService(IUnitOfWork unitOfWork, ILogger<FishingShipService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<FishingShipDto>> GetByIdAsync(int shipId, CancellationToken cancellationToken = default)
    {
        var ship = await _unitOfWork.FishingShips.GetByIdAsync(shipId, cancellationToken);
        
        if (ship == null)
            return Result<FishingShipDto>.Failure($"Fishing ship with ID {shipId} not found");

        var dto = MapToDto(ship);
        return Result<FishingShipDto>.Success(dto);
    }

    public async Task<Result<IEnumerable<FishingShipDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var ships = await _unitOfWork.FishingShips.GetAllAsync(cancellationToken);
        var dtos = ships.Select(MapToDto);
        return Result<IEnumerable<FishingShipDto>>.Success(dtos);
    }

    public async Task<Result<FishingShipDto>> CreateAsync(CreateFishingShipRequest request, CancellationToken cancellationToken = default)
    {
        // Check for duplicate IARA ID
        if (await _unitOfWork.FishingShips.AnyAsync(s => s.IaraIdNumber == request.IaraIdNumber, cancellationToken))
            return Result<FishingShipDto>.Failure($"Ship with IARA ID {request.IaraIdNumber} already exists");

        // Check for duplicate Maritime Number
        if (await _unitOfWork.FishingShips.AnyAsync(s => s.MaritimeNumber == request.MaritimeNumber, cancellationToken))
            return Result<FishingShipDto>.Failure($"Ship with Maritime Number {request.MaritimeNumber} already exists");

        var ship = new FishingShip
        {
            IaraIdNumber = request.IaraIdNumber,
            MaritimeNumber = request.MaritimeNumber,
            ShipName = request.ShipName,
            OwnerName = request.OwnerName,
            Tonnage = request.Tonnage,
            ShipLength = request.ShipLength,
            EnginePower = request.EnginePower,
            FuelType = request.FuelType,
            RegistrationDate = request.RegistrationDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.FishingShips.AddAsync(ship, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created fishing ship {ShipId} - {ShipName}", ship.ShipId, ship.ShipName);

        var dto = MapToDto(ship);
        return Result<FishingShipDto>.Success(dto);
    }

    public async Task<Result<FishingShipDto>> UpdateAsync(int shipId, UpdateFishingShipRequest request, CancellationToken cancellationToken = default)
    {
        var ship = await _unitOfWork.FishingShips.GetByIdAsync(shipId, cancellationToken);
        
        if (ship == null)
            return Result<FishingShipDto>.Failure($"Fishing ship with ID {shipId} not found");

        ship.ShipName = request.ShipName;
        ship.OwnerName = request.OwnerName;
        ship.Tonnage = request.Tonnage;
        ship.ShipLength = request.ShipLength;
        ship.EnginePower = request.EnginePower;
        ship.FuelType = request.FuelType;
        ship.IsActive = request.IsActive;
        ship.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.FishingShips.Update(ship);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated fishing ship {ShipId}", shipId);

        var dto = MapToDto(ship);
        return Result<FishingShipDto>.Success(dto);
    }

    public async Task<Result> DeleteAsync(int shipId, CancellationToken cancellationToken = default)
    {
        var ship = await _unitOfWork.FishingShips.GetByIdAsync(shipId, cancellationToken);
        
        if (ship == null)
            return Result.Failure($"Fishing ship with ID {shipId} not found");

        // Soft delete
        ship.IsActive = false;
        ship.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.FishingShips.Update(ship);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted (soft) fishing ship {ShipId}", shipId);

        return Result.Success();
    }

    private static FishingShipDto MapToDto(FishingShip ship)
    {
        return new FishingShipDto
        {
            ShipId = ship.ShipId,
            IaraIdNumber = ship.IaraIdNumber,
            MaritimeNumber = ship.MaritimeNumber,
            ShipName = ship.ShipName,
            OwnerName = ship.OwnerName,
            Tonnage = ship.Tonnage,
            ShipLength = ship.ShipLength,
            EnginePower = ship.EnginePower,
            FuelType = ship.FuelType,
            RegistrationDate = ship.RegistrationDate,
            IsActive = ship.IsActive
        };
    }
}
