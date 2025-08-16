using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Repositories.Interfaces;
using System.Linq.Expressions;

namespace RakbnyMa_aak.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IQueryable<T> GetAllQueryable()
        {
            return _dbSet.AsNoTracking();
        }
        public IQueryable<T> GetByIdQueryable(object id)
        {
            return _dbSet.Where(e => EF.Property<object>(e, "Id")!.Equals(id));
        }
        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
        public async Task<PaginatedResult<T>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _dbSet.CountAsync();

            var items = await _dbSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<T>(items, totalCount, pageNumber, pageSize);
        }
        public async Task<PaginatedResult<T>> GetPaginatedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
        {
            var query = _dbSet.Where(predicate);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<T>(items, totalCount, pageNumber, pageSize);
        }
        public async Task<List<TDto>> GetProjectedListAsync<TDto>(
            Expression<Func<T, bool>> predicate,
            IMapper mapper,
            CancellationToken cancellationToken)
                {
                    return await _dbSet
                        .Where(predicate)
                        .AsNoTracking()
                        .ProjectTo<TDto>(mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
                }

        public async Task<PaginatedResult<TDto>> GetProjectedPaginatedAsync<TDto>(
                Expression<Func<T, bool>> predicate,
                int page,
                int pageSize,
                IMapper mapper,
                CancellationToken cancellationToken)
                    {
                        var query = _dbSet
                            .Where(predicate)
                            .AsNoTracking();

                        var totalCount = await query.CountAsync(cancellationToken);

                        var items = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ProjectTo<TDto>(mapper.ConfigurationProvider)
                            .ToListAsync(cancellationToken);

                        return new PaginatedResult<TDto>(items, totalCount, page, pageSize);
                    }
        public async Task<TDto?> GetProjectedSingleAsync<TDto>(
            Expression<Func<T, bool>> predicate,
            IMapper mapper,
            CancellationToken cancellationToken)
                {
                    return await _dbSet
                        .Where(predicate)
                        .AsNoTracking()
                        .ProjectTo<TDto>(mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);
                }

    }

}