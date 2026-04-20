using TanitaTracker.Core.Entities;

namespace TanitaTracker.Core.Interfaces;

public interface IScanRepository
{
    Task<BodyCompositionScan?> GetByIdAsync(Guid id, string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BodyCompositionScan>> GetAllForUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<BodyCompositionScan> AddAsync(BodyCompositionScan scan, CancellationToken cancellationToken = default);
    Task UpdateAsync(BodyCompositionScan scan, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, string userId, CancellationToken cancellationToken = default);
}