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
    public class FishingLogEntryService : IFishingLogEntryService
    {
        private readonly IFishingLogEntryRepository _repository;

        public FishingLogEntryService(IFishingLogEntryRepository repository)
        {
            _repository = repository;
        }

        public async Task<FishingLogEntryResponseDTO> GetByIdAsync(long id)
        {
            var logEntry = await _repository.GetByIdAsync(id);
            if (logEntry == null)
                throw new KeyNotFoundException($"Fishing log entry with ID {id} not found");

            return MapToResponseDTO(logEntry);
        }

        public async Task<IEnumerable<FishingLogEntryResponseDTO>> GetAllAsync()
        {
            var logEntries = await _repository.GetAllAsync();
            return logEntries.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<FishingLogEntryResponseDTO>> GetFilteredAsync(FishingLogEntryFilter filter)
        {
            var logEntries = await _repository.GetFilteredAsync(filter);
            return logEntries.Select(MapToResponseDTO);
        }

        public async Task<FishingLogEntryResponseDTO> CreateAsync(CreateFishingLogEntryRequestDTO request)
        {
            // Validate unique constraint: one log per ship per day
            var existing = await _repository.GetByShipAndDateAsync(request.ShipId, request.LogDate);
            if (existing != null)
                throw new InvalidOperationException($"A log entry for ship {request.ShipId} on date {request.LogDate:yyyy-MM-dd} already exists");

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
                SubmittedAt = DateTime.UtcNow
            };

            var created = await _repository.AddAsync(logEntry);
            return MapToResponseDTO(created);
        }

        public async Task DeleteAsync(long id)
        {
            var logEntry = await _repository.GetByIdAsync(id);
            if (logEntry == null)
                throw new KeyNotFoundException($"Fishing log entry with ID {id} not found");

            await _repository.DeleteAsync(id);
        }

        public async Task<bool> SignLogEntryAsync(long id)
        {
            var logEntry = await _repository.GetByIdAsync(id);
            if (logEntry == null)
                throw new KeyNotFoundException($"Fishing log entry with ID {id} not found");

            if (logEntry.IsSigned)
                throw new InvalidOperationException("Log entry is already signed");

            logEntry.IsSigned = true;
            await _repository.UpdateAsync(logEntry);
            return true;
        }

        private FishingLogEntryResponseDTO MapToResponseDTO(FishingLogEntry logEntry)
        {
            return new FishingLogEntryResponseDTO
            {
                LogEntryId = logEntry.LogEntryId,
                ShipId = logEntry.ShipId,
                LogDate = logEntry.LogDate,
                StartTime = logEntry.StartTime,
                EndTime = logEntry.EndTime,
                FishingZone = logEntry.FishingZone,
                CatchDetails = logEntry.CatchDetails,
                RouteDetails = logEntry.RouteDetails,
                IsSigned = logEntry.IsSigned,
                SubmittedAt = logEntry.SubmittedAt
            };
        }
    }
}