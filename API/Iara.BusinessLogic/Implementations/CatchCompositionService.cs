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
    public class CatchCompositionService : ICatchCompositionService
    {
        private readonly IGenericRepository<CatchComposition> _repository;

        public CatchCompositionService(IGenericRepository<CatchComposition> repository)
        {
            _repository = repository;
        }

        public async Task<CatchCompositionResponseDTO> GetByIdAsync(long id)
        {
            var catchComposition = await _repository.GetByIdAsync(id);
            if (catchComposition == null)
                throw new KeyNotFoundException($"Catch composition with ID {id} not found");

            return MapToResponseDTO(catchComposition);
        }

        public async Task<IEnumerable<CatchCompositionResponseDTO>> GetByLogEntryIdAsync(long logEntryId)
        {
            var catches = await _repository.FindAsync(c => c.LogEntryId == logEntryId);
            return catches.Select(MapToResponseDTO);
        }

        public async Task<CatchCompositionResponseDTO> CreateAsync(CreateCatchCompositionRequestDTO request)
        {
            var catchComposition = new CatchComposition
            {
                LogEntryId = request.LogEntryId,
                FishSpecies = request.FishSpecies,
                WeightKg = request.WeightKg,
                Count = request.Count,
                Status = request.Status
            };

            var created = await _repository.AddAsync(catchComposition);
            return MapToResponseDTO(created);
        }

        public async Task DeleteAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }

        private CatchCompositionResponseDTO MapToResponseDTO(CatchComposition catchComposition)
        {
            return new CatchCompositionResponseDTO
            {
                CatchId = catchComposition.CatchId,
                LogEntryId = catchComposition.LogEntryId,
                FishSpecies = catchComposition.FishSpecies,
                WeightKg = catchComposition.WeightKg,
                Count = catchComposition.Count,
                Status = catchComposition.Status
            };
        }
    }
}