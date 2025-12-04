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
    public class InspectionService : IInspectionService
    {
        private readonly IInspectionRepository _repository;

        public InspectionService(IInspectionRepository repository)
        {
            _repository = repository;
        }

        public async Task<InspectionResponseDTO> GetByIdAsync(int id)
        {
            var inspection = await _repository.GetByIdAsync(id);
            if (inspection == null)
                throw new KeyNotFoundException($"Inspection with ID {id} not found");

            return MapToResponseDTO(inspection);
        }

        public async Task<IEnumerable<InspectionResponseDTO>> GetAllAsync()
        {
            var inspections = await _repository.GetAllAsync();
            return inspections.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<InspectionResponseDTO>> GetFilteredAsync(InspectionFilter filter)
        {
            var inspections = await _repository.GetFilteredAsync(filter);
            return inspections.Select(MapToResponseDTO);
        }

        public async Task<InspectionResponseDTO> CreateAsync(CreateInspectionRequestDTO request)
        {
            var existing = await _repository.GetByProtocolNumberAsync(request.ProtocolNumber);
            if (existing != null)
                throw new InvalidOperationException($"An inspection with protocol number {request.ProtocolNumber} already exists");

            var inspection = new Inspection
            {
                InspectorId = request.InspectorId,
                ShipId = request.ShipId,
                InspectionDate = request.InspectionDate,
                InspectionLocation = request.InspectionLocation,
                ProtocolNumber = request.ProtocolNumber,
                HasViolation = request.HasViolation,
                ViolationDescription = request.ViolationDescription,
                SanctionsImposed = request.SanctionsImposed,
                ProofOfViolationUrl = request.ProofOfViolationUrl,
                IsProcessed = false
            };

            var created = await _repository.AddAsync(inspection);
            return MapToResponseDTO(created);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<bool> ProcessInspectionAsync(int id)
        {
            var inspection = await _repository.GetByIdAsync(id);
            if (inspection == null)
                throw new KeyNotFoundException($"Inspection with ID {id} not found");

            if (inspection.IsProcessed)
                throw new InvalidOperationException("Inspection is already processed");

            inspection.IsProcessed = true;
            await _repository.UpdateAsync(inspection);
            return true;
        }

        private InspectionResponseDTO MapToResponseDTO(Inspection inspection)
        {
            return new InspectionResponseDTO
            {
                InspectionId = inspection.InspectionId,
                InspectorId = inspection.InspectorId,
                ShipId = inspection.ShipId,
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
}