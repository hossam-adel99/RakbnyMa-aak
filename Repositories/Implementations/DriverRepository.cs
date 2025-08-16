using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Interfaces;

namespace RakbnyMa_aak.Repositories.Implementations
{
    public class DriverRepository : GenericRepository<Driver>, IDriverRepository
    {
        private readonly AppDbContext _context;

        public DriverRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Driver?> GetByUserIdAsync(string userId)
        {
            return await _context.Drivers
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public IQueryable<Driver> GetPendingApprovalDriversQueryable()
        {
            return _context.Drivers
                .AsNoTracking()
                .Where(d => !d.IsApproved);
        }
    }
}
