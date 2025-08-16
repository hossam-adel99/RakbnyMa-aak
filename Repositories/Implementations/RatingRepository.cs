using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Interfaces;

namespace RakbnyMa_aak.Repositories.Implementations
{
    public class RatingRepository: GenericRepository<Rating>, IRatingRepository
    {
        private readonly AppDbContext _context;

        public RatingRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
