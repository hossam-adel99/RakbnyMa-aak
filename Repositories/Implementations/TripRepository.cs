using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripExists;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Interfaces;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Repositories.Implementations
{
    public class TripRepository : GenericRepository<Trip>, ITripRepository
    {
        public TripRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<string?> GetDriverIdByTripIdAsync(int tripId)
        {
            return await GetAllQueryable()
                .Where(t => t.Id == tripId)
                .Select(t => t.DriverId)
                .FirstOrDefaultAsync();
        }
        public async Task<TripValidationResultDto?> GetTripValidationDtoAsync(int tripId)
        {
            return await _context.Trips
                .Include(t => t.Driver)
                    .ThenInclude(d => d.User)
                .Where(t => t.Id == tripId && !t.IsDeleted && t.TripStatus != TripStatus.Cancelled)
                .Select(t => new TripValidationResultDto
                {
                    TripId = t.Id,
                    DriverId = t.DriverId,
                    DriverFullName = t.Driver.User.FullName,
                    DriverPicture = t.Driver.User.Picture,
                    TripDate = t.TripDate,
                    AvailableSeats = t.AvailableSeats,
                    PricePerSeat = t.PricePerSeat,
                    IsDeleted = t.IsDeleted
                })
                .FirstOrDefaultAsync();
        }


    }
}
