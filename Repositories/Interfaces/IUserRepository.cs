using RakbnyMa_aak.DTOs.UserDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.Repositories.Interfaces
{
    public interface IUserRepository :IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<ApplicationUser?> GetByUsernameAsync(string username);
        IQueryable<ApplicationUser> GetUsersQueryable();
    }
}