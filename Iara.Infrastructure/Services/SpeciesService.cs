using Iara.Application.Common;
using Iara.Application.DTOs.Species;
using Iara.Application.Services;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;

namespace Iara.Infrastructure.Services;

public class SpeciesService : ISpeciesService
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SpeciesService(ISpeciesRepository speciesRepository, IUnitOfWork unitOfWork)
    {
        _speciesRepository = speciesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SpeciesDto>> GetByIdAsync(int speciesId, CancellationToken cancellationToken = default)
    {
        var species = await _speciesRepository.GetByIdAsync(speciesId, cancellationToken);
        if (species == null)
            return Result<SpeciesDto>.Failure("Species not found");

        return Result<SpeciesDto>.Success(MapToDto(species));
    }

    public async Task<Result<IEnumerable<SpeciesDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var speciesList = await _speciesRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<SpeciesDto>>.Success(speciesList.Select(MapToDto));
    }

    public async Task<Result<SpeciesDto>> CreateAsync(CreateSpeciesRequest request, CancellationToken cancellationToken = default)
    {
        var existingSpecies = await _speciesRepository.GetByNameAsync(request.SpeciesName, cancellationToken);
        if (existingSpecies != null)
            return Result<SpeciesDto>.Failure("A species with this name already exists");

        var species = new Species
        {
            SpeciesName = request.SpeciesName
        };

        await _speciesRepository.AddAsync(species, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<SpeciesDto>.Success(MapToDto(species));
    }

    public async Task<Result> DeleteAsync(int speciesId, CancellationToken cancellationToken = default)
    {
        var species = await _speciesRepository.GetByIdAsync(speciesId, cancellationToken);
        if (species == null)
            return Result.Failure("Species not found");

        _speciesRepository.Remove(species);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static SpeciesDto MapToDto(Species species)
    {
        return new SpeciesDto
        {
            SpeciesId = species.SpeciesId,
            SpeciesName = species.SpeciesName
        };
    }
}
