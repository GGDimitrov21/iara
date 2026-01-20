using Iara.Domain.Entities;

namespace Iara.Domain.Repositories;

public interface IPersonnelRepository : IRepository<Personnel>
{
    Task<Personnel?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Personnel>> GetByRoleAsync(string role, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}

public interface IVesselRepository : IRepository<Vessel>
{
    Task<Vessel?> GetByRegNumberAsync(string regNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vessel>> GetByCaptainIdAsync(int captainId, CancellationToken cancellationToken = default);
    Task<Vessel?> GetWithDetailsAsync(int vesselId, CancellationToken cancellationToken = default);
}

public interface IPermitRepository : IRepository<Permit>
{
    Task<IEnumerable<Permit>> GetByVesselIdAsync(int vesselId, CancellationToken cancellationToken = default);
    Task<Permit?> GetActivePermitForVesselAsync(int vesselId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permit>> GetActivePermitsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Permit>> GetExpiringPermitsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
}

public interface ISpeciesRepository : IRepository<Species>
{
    Task<Species?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}

public interface ICatchQuotaRepository : IRepository<CatchQuota>
{
    Task<IEnumerable<CatchQuota>> GetByPermitIdAsync(int permitId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CatchQuota>> GetBySpeciesIdAsync(int speciesId, CancellationToken cancellationToken = default);
    Task<CatchQuota?> GetByPermitSpeciesYearAsync(int permitId, int speciesId, short year, CancellationToken cancellationToken = default);
}

public interface ILogbookRepository : IRepository<Logbook>
{
    Task<IEnumerable<Logbook>> GetByVesselIdAsync(int vesselId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Logbook>> GetByCaptainIdAsync(int captainId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Logbook>> GetByDateRangeAsync(int vesselId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<Logbook?> GetWithDetailsAsync(int logEntryId, CancellationToken cancellationToken = default);
}

public interface IInspectionRepository : IRepository<Inspection>
{
    Task<IEnumerable<Inspection>> GetByVesselIdAsync(int vesselId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Inspection>> GetByInspectorIdAsync(int inspectorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Inspection>> GetIllegalInspectionsAsync(CancellationToken cancellationToken = default);
    Task<Inspection?> GetWithDetailsAsync(int inspectionId, CancellationToken cancellationToken = default);
}

public interface ITicketRepository : IRepository<Ticket>
{
    Task<Ticket?> GetByTicketNumberAsync(string ticketNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Ticket>> GetByInspectionIdAsync(int inspectionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Ticket>> GetUnvalidatedTicketsAsync(CancellationToken cancellationToken = default);
}
