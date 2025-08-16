using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Interfaces;
using System.Linq.Expressions;
using RakbnyMa_aak.CQRS.Features.Cities;

namespace RakbnyMa_aak.Repositories.Implementations
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        public CityRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<City>> GetAllAsync(Expression<Func<City, bool>> filter)
        {
            return await _context.Set<City>().Where(filter).ToListAsync();
        }
        public async Task<City> GetCityWithGovernorateNameByIdAsync(int cityId)
        {
            return await _context.Cities
                .Include(c => c.Governorate)
                .FirstOrDefaultAsync(c => c.Id == cityId);
        }
        public async Task<List<CityDto>> GetCitiesDtoByGovernorateIdAsync(int governorateId, IMapper mapper, CancellationToken cancellationToken)
        {
            return await _context.Cities
                .Where(c => c.GovernorateId == governorateId)
                .ProjectTo<CityDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<string?> GetCityNameByIdAsync(int cityId)
        {
            return await _context.Cities
                .AsNoTracking()
                .Where(c => c.Id == cityId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();
        }

    }
}
