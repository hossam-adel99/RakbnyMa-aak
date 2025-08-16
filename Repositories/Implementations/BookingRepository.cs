using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Interfaces;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Repositories.Implementations
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Booking> GetBookingsByTripIdQueryable(int tripId)
        {
            return _context.Bookings
                .AsNoTracking()
                .Where(b => b.TripId == tripId && !b.IsDeleted);
        }

        public IQueryable<Booking> GetBookingsByUserIdQueryable(string userId)
        {
            return _context.Bookings
                .AsNoTracking()
                .Include(b => b.Trip)
                .Where(b => b.UserId == userId && !b.IsDeleted);
        }

        public async Task<Booking?> GetBookingDetailsAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Trip)
                    .ThenInclude(t => t.Driver)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == bookingId && !b.IsDeleted);
        }

        public async Task<bool> IsUserAlreadyBookedAsync(string userId, int tripId)
        {
            return await _context.Bookings
                .AsNoTracking()
                .AnyAsync(b => b.TripId == tripId && b.UserId == userId && !b.IsDeleted);
        }

        public async Task<int> GetTotalSeatsBookedAsync(int tripId)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Where(b => b.TripId == tripId && !b.IsDeleted)
                .SumAsync(b => b.NumberOfSeats);
        }
        public IQueryable<Booking> GetConfirmedFinishedBookingQueryable(int tripId, string userId)
        {
            return _context.Bookings
                .AsNoTracking()
                .Where(b => b.TripId == tripId &&
                            b.UserId == userId &&
                            b.HasEnded &&
                            b.RequestStatus == RequestStatus.Confirmed &&
                            !b.IsDeleted);
        }
        public async Task<Booking?> GetBookingByUserAndTripAsync(string userId, int tripId)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.TripId == tripId && !b.IsDeleted);
        }

    }
}
