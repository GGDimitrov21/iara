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
    public class FishingPermitService : IFishingPermitService
    {
        private readonly IFishingPermitRepository _repository;

        public FishingPermitService(IFishingPermitRepository repository)
        {
            _repository = repository;
        }

        public async Task<FishingPermitResponseDTO> GetByIdAsync(int id)
        {
            var permit = await _repository.GetByIdAsync(id);
            if (permit == null)
                throw new KeyNotFoundException($"Fishing permit with ID {id} not found");

            return MapToResponseDTO(permit);
        }

        public async Task<IEnumerable<FishingPermitResponseDTO>> GetAllAsync()
        {
            var permits = await _repository.GetAllAsync();
            return permits.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<FishingPermitResponseDTO>> GetFilteredAsync(FishingPermitFilter filter)
        {
            var permits = await _repository.GetFilteredAsync(filter);
            return permits.Select(MapToResponseDTO);
        }

        public async Task<FishingPermitResponseDTO> CreateAsync(CreateFishingPermitRequestDTO request)
        {
            var permit = new FishingPermit
            {
                ShipId = request.ShipId,
                PermitYear = request.PermitYear,
                IssueDate = request.IssueDate,
                ValidFrom = request.ValidFrom,
                ValidUntil = request.ValidUntil,
                CatchQuotaType = request.CatchQuotaType,
                MinAnnualCatch = request.MinAnnualCatch,
                MaxAnnualCatch = request.MaxAnnualCatch,
                TotalHoursAnnualLimit = request.TotalHoursAnnualLimit,
                Status = request.Status,
                RegistrationDocumentId = request.RegistrationDocumentId,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.AddAsync(permit);
            return MapToResponseDTO(created);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private FishingPermitResponseDTO MapToResponseDTO(FishingPermit permit)
        {
            return new FishingPermitResponseDTO
            {
                PermitId = permit.PermitId,
                ShipId = permit.ShipId,
                PermitYear = permit.PermitYear,
                IssueDate = permit.IssueDate,
                ValidFrom = permit.ValidFrom,
                ValidUntil = permit.ValidUntil,
                CatchQuotaType = permit.CatchQuotaType,
                MinAnnualCatch = permit.MinAnnualCatch,
                MaxAnnualCatch = permit.MaxAnnualCatch,
                TotalHoursAnnualLimit = permit.TotalHoursAnnualLimit,
                Status = permit.Status,
                RegistrationDocumentId = permit.RegistrationDocumentId,
                CreatedAt = permit.CreatedAt
            };
        }
    }
}