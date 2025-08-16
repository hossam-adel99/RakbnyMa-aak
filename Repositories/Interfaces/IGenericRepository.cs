using AutoMapper;
using RakbnyMa_aak.GeneralResponse;
using System.Linq.Expressions;

namespace RakbnyMa_aak.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAllQueryable();
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> CompleteAsync();
        IQueryable<T> GetByIdQueryable(object id);
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<PaginatedResult<T>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<PaginatedResult<T>> GetPaginatedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);
        Task<List<TDto>> GetProjectedListAsync<TDto>(
            Expression<Func<T, bool>> predicate,
            IMapper mapper,
            CancellationToken cancellationToken);

        Task<PaginatedResult<TDto>> GetProjectedPaginatedAsync<TDto>(
            Expression<Func<T, bool>> predicate,
            int page,
            int pageSize,
            IMapper mapper,
            CancellationToken cancellationToken);

        Task<TDto?> GetProjectedSingleAsync<TDto>(
             Expression<Func<T, bool>> predicate,
             IMapper mapper,
             CancellationToken cancellationToken);


    }
}