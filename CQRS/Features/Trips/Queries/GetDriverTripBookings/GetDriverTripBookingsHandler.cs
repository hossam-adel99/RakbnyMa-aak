using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Trip.Queries.GetDriverTripBookings
{
    public class GetDriverTripBookingsHandler : IRequestHandler<GetDriverTripBookingsQuery, Response<List<BookingForDriverDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDriverTripBookingsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<List<BookingForDriverDto>>> Handle(GetDriverTripBookingsQuery request, CancellationToken cancellationToken)
        {

            var driver = await _unitOfWork.DriverRepository.GetByUserIdAsync(request.DriverUserId);
            if (driver == null)
                return Response<List<BookingForDriverDto>>.Fail("أنت لست مسجلاً كسائق.");


            var trips = await _unitOfWork.TripRepository
                .GetAllQueryable()
                .Where(t => t.DriverId == driver.UserId)
                .Include(t => t.Bookings)
                    .ThenInclude(b => b.User)
                .ToListAsync();


            var bookings = trips
                .SelectMany(t => t.Bookings.Select(b => new BookingForDriverDto
                {
                    BookingId = b.Id,
                    TripId = b.TripId,
                    UserId = b.UserId,
                    UserName = b.User.FullName,
                    Status = b.RequestStatus.ToString(),
                    RequestDate = b.CreatedAt
                }))
                .ToList();

            return Response<List<BookingForDriverDto>>.Success(bookings);
        }
    }
}
