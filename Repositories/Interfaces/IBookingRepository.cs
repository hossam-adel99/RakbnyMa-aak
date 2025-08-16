using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.Repositories.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        IQueryable<Booking> GetBookingsByTripIdQueryable(int tripId);
        IQueryable<Booking> GetBookingsByUserIdQueryable(string userId);
        Task<Booking?> GetBookingDetailsAsync(int bookingId);
        Task<bool> IsUserAlreadyBookedAsync(string userId, int tripId);
        Task<int> GetTotalSeatsBookedAsync(int tripId);
        IQueryable<Booking> GetConfirmedFinishedBookingQueryable(int tripId, string userId);
        Task<Booking?> GetBookingByUserAndTripAsync(string userId, int tripId);


    }
}
