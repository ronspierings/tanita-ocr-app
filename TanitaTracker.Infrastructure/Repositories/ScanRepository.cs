using Microsoft.EntityFrameworkCore;
using TanitaTracker.Core.Entities;
using TanitaTracker.Core.Interfaces;
using TanitaTracker.Infrastructure.Data;

namespace TanitaTracker.Infrastructure.Repositories
{
    public class ScanRepository : IScanRepository
    {
        private readonly ApplicationDbContext _context;

        public ScanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lookup one specic BodyScanComposition (using ID)
        /// /// Note: For Row Level Security we require a userId
        /// </summary>
        /// <param name="id">BodyScanCompositionId (GUI)</param>
        /// <param name="userId">The current users userId as </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<BodyCompositionScan?> GetByIdAsync(Guid id, String userId, CancellationToken cancellationToken = default)
        {
            // LINQ Select when both the BodyScanComposition ID and the UserId match within a scan
            return await _context.BodyCompositionScans.FirstOrDefaultAsync(scan => 
                scan.Id == id && 
                scan.UserId == userId, 
            cancellationToken);
        }

        /// <summary>
        /// Retrieve all BodyCompositionScans[] from the given user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BodyCompositionScan>> GetAllForUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.BodyCompositionScans
                .Where(scan => scan.UserId == userId)
                .OrderByDescending(s => s.ScanDate)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Add one specific new scan to the Storage
        /// </summary>
        /// <param name="scan">The specific BodyCompositionScan to add</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<BodyCompositionScan> AddAsync(BodyCompositionScan scan, CancellationToken cancellationToken = default)
        {
            _context.BodyCompositionScans.Add(scan);
            await _context.SaveChangesAsync(cancellationToken);
            return scan;
        }

        /// <summary>
        /// Update a existing BodyCompositionScan. 
        /// Note: No return value is given, so "no news is good news"
        /// </summary>
        /// <param name="scan"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UpdateAsync(BodyCompositionScan scan, CancellationToken cancellationToken = default )
        {
            _context.BodyCompositionScans.Update(scan);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Remove a specific BodyCompositionScan. 
        /// Note: For Row Level Security we require a userId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id, string userId, CancellationToken cancellationToken = default)
        {
            var scan = await GetByIdAsync(id, userId, cancellationToken);

            if(scan != null)
            {
                _context.BodyCompositionScans.Remove(scan);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
