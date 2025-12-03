namespace Iara.Application.Abstractions;

using Iara.Domain.Entities;

public interface IVesselRepository
{
    Task<Vessel?> GetByIdAsync(int id, CancellationToken ct = default);
}
