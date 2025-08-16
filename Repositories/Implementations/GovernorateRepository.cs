using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Interfaces;

namespace RakbnyMa_aak.Repositories.Implementations
{
    public class GovernorateRepository : GenericRepository<Governorate>, IGovernorateRepository
    {
        public GovernorateRepository(AppDbContext context) : base(context)
        {

        }
     

    }
}
