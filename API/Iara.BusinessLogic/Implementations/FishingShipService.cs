using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iara.BusinessLogic.Services;
using Iara.DomainModel.Entities;
using Iara.DomainModel.RequestDTOs;
using Iara.DomainModel.ResponseDTOs;
using Iara.DomainModel.Filters;
using Iara.Infrastructure.Repositories;

namespace Iara.BusinessLogic.Implementations
{
    public class FishingShipService : IFishingShipService
    {
        private readonly IFishingShipRepository _repository;

        public FishingShipService(IFishingShipRepository repository)
        {
            _repository = repository;
        }

        public async Task<FishingShipResponseDTO> GetByIdAsync(int id)
        {
            var ship = await _repository.GetByIdAsync(id);
            if (ship == null)
                throw new KeyNotFoundException($"Fishing ship with ID {id} not found");

            return MapToResponseDTO(ship);
        }

        public async Task<IEnumerable<FishingShipResponseDTO>> GetAllAsync()
        {
            var ships = await _repository.GetAllAsync();
            return ships.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<FishingShipResponseDTO>> GetFilteredAsync(FishingShipFilter filter)
        {
            var ships = await _repository.GetFilteredAsync(filter);
            return ships.Select(MapToResponseDTO);
        }

        public async Task<FishingShipResponseDTO> CreateAsync(CreateFishingShipRequestDTO request)
        {
            // Validate unique constraints
            var existingByIara = await _repository.GetByIaraIdNumberAsync(request.IaraIdNumber);
            if (existingByIara != null)
                throw new InvalidOperationException($"A ship with IARA ID {request.IaraIdNumber} already exists");

            var existingByMaritime = await _repository.GetByMaritimeNumberAsync(request.MaritimeNumber);
            if (existingByMaritime != null)
                throw new InvalidOperationException($"A ship with Maritime Number {request.MaritimeNumber} already exists");

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
                RegistrationDocumentId = request.RegistrationDocumentId,
                RegistrationDate = request.RegistrationDate,
                IsActive = true,
                LastUpdated = DateTime.UtcNow
            };

            var created = await _repository.AddAsync(ship);
            return MapToResponseDTO(created);
        }

        public async Task<FishingShipResponseDTO> UpdateAsync(int id, UpdateFishingShipRequestDTO request)
        {
            var ship = await _repository.GetByIdAsync(id);
            if (ship == null)
                throw new KeyNotFoundException($"Fishing ship with ID {id} not found");

            if (!string.IsNullOrEmpty(request.ShipName))
                ship.ShipName = request.ShipName;

            if (!string.IsNullOrEmpty(request.OwnerName))
                ship.OwnerName = request.OwnerName;

            if (request.Tonnage.HasValue)
                ship.Tonnage = request.Tonnage.Value;

            if (request.ShipLength.HasValue)
                ship.ShipLength = request.ShipLength.Value;

            if (request.EnginePower.HasValue)
                ship.EnginePower = request.EnginePower.Value;

            if (!string.IsNullOrEmpty(request.FuelType))
                ship.FuelType = request.FuelType;

            if (request.IsActive.HasValue)
                ship.IsActive = request.IsActive.Value;

            ship.LastUpdated = DateTime.UtcNow;

            await _repository.UpdateAsync(ship);
            return MapToResponseDTO(ship);
        }

        public async Task DeleteAsync(int id)
        {
            var ship = await _repository.GetByIdAsync(id);
            if (ship == null)
                throw new KeyNotFoundException($"Fishing ship with ID {id} not found");

            await _repository.DeleteAsync(id);
        }

        private FishingShipResponseDTO MapToResponseDTO(FishingShip ship)
        {
            return new FishingShipResponseDTO
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
                RegistrationDocumentId = ship.RegistrationDocumentId,
                RegistrationDate = ship.RegistrationDate,
                IsActive = ship.IsActive,
                LastUpdated = ship.LastUpdated
            };
        }
    }
}