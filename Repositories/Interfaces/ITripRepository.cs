using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripExists;
using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.Repositories.Interfaces
{
    public interface ITripRepository : IGenericRepository<Trip>
    {
        Task<string?> GetDriverIdByTripIdAsync(int tripId);
        Task<TripValidationResultDto?> GetTripValidationDtoAsync(int tripId);
    }
}
