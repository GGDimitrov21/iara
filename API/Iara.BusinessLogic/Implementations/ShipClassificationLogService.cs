using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iara.BusinessLogic.Services;
using Iara.DomainModel.Entities;
using Iara.DomainModel.RequestDTOs;
using Iara.DomainModel.ResponseDTOs;
using Iara.Infrastructure.Base;

namespace Iara.BusinessLogic.Implementations
{
    public class ShipClassificationLogService : IShipClassificationLogService
    {
        private readonly IGenericRepository<ShipClassificationLog> _repository;

        public ShipClassificationLogService(IGenericRepository<ShipClassificationLog> repository)
        {
            _repository = repository;
        }

        public async Task<ShipClassificationLogResponseDTO> GetByIdAsync(int id)
        {
            var log = await _repository.GetByIdAsync(id);
            if (log == null)
                throw new KeyNotFoundException($"Ship classification log with ID {id} not found");

            return MapToResponseDTO(log);
        }

        public async Task<IEnumerable<ShipClassificationLogResponseDTO>> GetByShipIdAsync(int shipId)
        {
            var logs = await _repository.FindAsync(l => l.ShipId == shipId);
            return logs.Select(MapToResponseDTO);
        }

        public async Task<ShipClassificationLogResponseDTO> CreateAsync(CreateShipClassificationLogRequestDTO request)
        {
            // Check if log already exists for this ship and year
            var existing = await _repository.ExistsAsync(l => l.ShipId == request.ShipId && l.ClassificationYear == request.ClassificationYear);
            if (existing)
                throw new InvalidOperationException($"A classification log for ship {request.ShipId} and year {request.ClassificationYear} already exists");

            var log = new ShipClassificationLog
            {
                ShipId = request.ShipId,
                ClassificationYear = request.ClassificationYear,
                TotalEngineHours = request.TotalEngineHours,
                ClassificationLevel = request.ClassificationLevel,
                ClassificationDate = request.ClassificationDate ?? DateTime.Today
            };

            var created = await _repository.AddAsync(log);
            return MapToResponseDTO(created);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private ShipClassificationLogResponseDTO MapToResponseDTO(ShipClassificationLog log)
        {
            return new ShipClassificationLogResponseDTO
            {
                LogId = log.LogId,
                ShipId = log.ShipId,
                ClassificationYear = log.ClassificationYear,
                TotalEngineHours = log.TotalEngineHours,
                ClassificationLevel = log.ClassificationLevel,
                ClassificationDate = log.ClassificationDate
            };
        }
    }
}