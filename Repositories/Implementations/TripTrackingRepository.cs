using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Interfaces;

namespace RakbnyMa_aak.Repositories.Implementations
{
    public class TripTrackingRepository : GenericRepository<TripTracking>, ITripTrackingRepository
    {
        public TripTrackingRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<TripTracking?> GetLastLocationAsync(int tripId)
        {
            return await _context.TripTrackings
                .Where(x => x.TripId == tripId)
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefaultAsync();
        }
    }
}
