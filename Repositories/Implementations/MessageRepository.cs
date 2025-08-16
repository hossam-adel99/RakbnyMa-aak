using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Interfaces;

namespace RakbnyMa_aak.Repositories.Implementations
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetMessagesByTripIdAsync(int tripId)
        {
            return await _context.Messages
                .Where(m => m.TripId == tripId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
    }

}
