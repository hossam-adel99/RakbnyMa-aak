using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.Repositories.Interfaces
{
    public interface ITripTrackingRepository : IGenericRepository<TripTracking>
    {
        Task<TripTracking?> GetLastLocationAsync(int tripId);
    }
}
