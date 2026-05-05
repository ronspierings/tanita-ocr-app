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
    }
}
