using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Implementations;
using RakbnyMa_aak.Repositories.Interfaces;
using RakbnyMa_aak.Services.Interfaces;

namespace RakbnyMa_aak.UOW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        public IDriverRepository DriverRepository { get; }
        public IUserRepository UserRepository { get; }
        public IBookingRepository BookingRepository { get; }
        public INotificationRepository NotificationRepository { get; }

        public IRatingRepository RatingRepository { get; }

        public IMessageRepository MessageRepository { get; private set; }


        public ITripRepository TripRepository { get; }

        public IGovernorateRepository GovernorateRepository { get; }

        public ICityRepository CityRepository { get; }
        public ITripTrackingRepository TripTrackingRepository { get; }

        public UnitOfWork(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            ICloudinaryService cloudinaryService,
            ITripRepository tripRepository,
            IGovernorateRepository governorateRepository,
            ICityRepository cityRepository,
            IRatingRepository ratingRepository,
            IMapper mapper
        )
        {
            _context = context;
            DriverRepository = new DriverRepository(_context);
            UserRepository = new UserRepository(_context);
            BookingRepository = new BookingRepository(_context);
            TripRepository = new TripRepository(_context);
            NotificationRepository= new NotificationRepository(_context);
            MessageRepository= new MessageRepository(_context);
            GovernorateRepository = new GovernorateRepository(_context);
            CityRepository = new CityRepository(_context);
            RatingRepository= new RatingRepository(_context);
            TripTrackingRepository = new TripTrackingRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task RollbackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
