using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.Repositories.Interfaces
{
    public interface IDriverRepository : IGenericRepository<Driver>
    {
        Task<Driver?> GetByUserIdAsync(string userId);
        IQueryable<Driver> GetPendingApprovalDriversQueryable();
    }

}
