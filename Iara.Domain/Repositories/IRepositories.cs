using Iara.Domain.Entities;

namespace Iara.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}

public interface IFishingShipRepository : IRepository<FishingShip>
{
    Task<FishingShip?> GetByIaraIdAsync(string iaraId, CancellationToken cancellationToken = default);
    Task<FishingShip?> GetByMaritimeNumberAsync(string maritimeNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<FishingShip>> GetActiveShipsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<FishingShip>> GetShipsByOwnerAsync(string ownerName, CancellationToken cancellationToken = default);
}

public interface IRegistrationRepository : IRepository<Registration>
{
    Task<IEnumerable<Registration>> GetValidRegistrationsAsync(DateOnly asOfDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Registration>> GetExpiringRegistrationsAsync(DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default);
}

public interface IFishingPermitRepository : IRepository<FishingPermit>
{
    Task<IEnumerable<FishingPermit>> GetPermitsByShipIdAsync(int shipId, CancellationToken cancellationToken = default);
    Task<FishingPermit?> GetActivePermitForShipAsync(int shipId, int year, CancellationToken cancellationToken = default);
    Task<IEnumerable<FishingPermit>> GetPermitsByYearAsync(int year, CancellationToken cancellationToken = default);
    Task<IEnumerable<FishingPermit>> GetExpiringPermitsAsync(DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default);
}

public interface IFishingLogEntryRepository : IRepository<FishingLogEntry>
{
    Task<IEnumerable<FishingLogEntry>> GetLogEntriesByShipIdAsync(int shipId, CancellationToken cancellationToken = default);
    Task<FishingLogEntry?> GetLogEntryWithCatchesAsync(long logEntryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<FishingLogEntry>> GetLogEntriesByDateRangeAsync(int shipId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    Task<bool> LogEntryExistsForDateAsync(int shipId, DateOnly date, CancellationToken cancellationToken = default);
}

public interface ICatchCompositionRepository : IRepository<CatchComposition>
{
    Task<IEnumerable<CatchComposition>> GetCatchesByLogEntryIdAsync(long logEntryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CatchComposition>> GetCatchesBySpeciesAsync(string species, CancellationToken cancellationToken = default);
}

public interface IInspectionRepository : IRepository<Inspection>
{
    Task<IEnumerable<Inspection>> GetInspectionsByShipIdAsync(int shipId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Inspection>> GetInspectionsByInspectorIdAsync(int inspectorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Inspection>> GetViolationsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Inspection>> GetUnprocessedInspectionsAsync(CancellationToken cancellationToken = default);
    Task<Inspection?> GetByProtocolNumberAsync(string protocolNumber, CancellationToken cancellationToken = default);
}

public interface IShipClassificationLogRepository : IRepository<ShipClassificationLog>
{
    Task<IEnumerable<ShipClassificationLog>> GetClassificationsByShipIdAsync(int shipId, CancellationToken cancellationToken = default);
    Task<ShipClassificationLog?> GetClassificationByYearAsync(int shipId, int year, CancellationToken cancellationToken = default);
}

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task RevokeAllUserTokensAsync(int userId, CancellationToken cancellationToken = default);
    Task RemoveExpiredTokensAsync(CancellationToken cancellationToken = default);
}
