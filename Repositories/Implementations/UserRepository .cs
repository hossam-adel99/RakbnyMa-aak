using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Interfaces;

namespace RakbnyMa_aak.Repositories.Implementations
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public IQueryable<ApplicationUser> GetUsersQueryable()
        {
            return _context.Users.AsNoTracking();
        }
    }
}
