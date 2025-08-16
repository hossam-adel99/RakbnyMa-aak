using RakbnyMa_aak.Repositories.Implementations;
using RakbnyMa_aak.Repositories.Interfaces;

namespace RakbnyMa_aak.UOW
{
    public interface IUnitOfWork
    {
        IDriverRepository DriverRepository {  get; }
        IUserRepository UserRepository { get; }
        IBookingRepository BookingRepository { get; }
        ITripRepository TripRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IGovernorateRepository GovernorateRepository { get; }
        ICityRepository CityRepository { get; }
        IRatingRepository RatingRepository { get; }
        IMessageRepository MessageRepository { get; }
        ITripTrackingRepository TripTrackingRepository {  get; }

        Task<int> CompleteAsync(); // SaveChanges
        Task RollbackAsync(); // Rollback 

    }
}
