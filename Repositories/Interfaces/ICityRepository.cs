using RakbnyMa_aak.Models;
using System.Linq.Expressions;

namespace RakbnyMa_aak.Repositories.Interfaces
{
    public interface ICityRepository : IGenericRepository<City>
    {
        Task<IEnumerable<City>> GetAllAsync(Expression<Func<City, bool>> filter);
        Task<City> GetCityWithGovernorateNameByIdAsync(int cityId);
        Task<string?> GetCityNameByIdAsync(int cityId);


    }
}
