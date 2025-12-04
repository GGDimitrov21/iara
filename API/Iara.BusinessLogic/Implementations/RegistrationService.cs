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
    public class RegistrationService : IRegistrationService
    {
        private readonly IGenericRepository<Registration> _repository;

        public RegistrationService(IGenericRepository<Registration> repository)
        {
            _repository = repository;
        }

        public async Task<RegistrationResponseDTO> GetByIdAsync(int id)
        {
            var registration = await _repository.GetByIdAsync(id);
            if (registration == null)
                throw new KeyNotFoundException($"Registration with ID {id} not found");

            return MapToResponseDTO(registration);
        }

        public async Task<IEnumerable<RegistrationResponseDTO>> GetAllAsync()
        {
            var registrations = await _repository.GetAllAsync();
            return registrations.Select(MapToResponseDTO);
        }

        public async Task<RegistrationResponseDTO> CreateAsync(CreateRegistrationRequestDTO request)
        {
            var registration = new Registration
            {
                DocumentType = request.DocumentType,
                IssuedBy = request.IssuedBy,
                IssueDate = request.IssueDate,
                ValidFrom = request.ValidFrom,
                ValidUntil = request.ValidUntil,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.AddAsync(registration);
            return MapToResponseDTO(created);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private RegistrationResponseDTO MapToResponseDTO(Registration registration)
        {
            return new RegistrationResponseDTO
            {
                RegistrationId = registration.RegistrationId,
                DocumentType = registration.DocumentType,
                IssuedBy = registration.IssuedBy,
                IssueDate = registration.IssueDate,
                ValidFrom = registration.ValidFrom,
                ValidUntil = registration.ValidUntil,
                Description = registration.Description,
                CreatedAt = registration.CreatedAt
            };
        }
    }
}