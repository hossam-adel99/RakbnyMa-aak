using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.Repositories.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesByTripIdAsync(int tripId);
    }

}
