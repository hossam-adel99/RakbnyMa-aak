using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using System.Security.Claims;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Queries.Driver.GetPendingBookingsForDriver
{
    public class GetPendingBookingsForDriverHandler
        : IRequestHandler<GetPendingBookingsForDriverQuery, Response<List<PendingBookingForDriverDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPendingBookingsForDriverHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<PendingBookingForDriverDto>>> Handle(
            GetPendingBookingsForDriverQuery request,
            CancellationToken cancellationToken)
        {
            var driverUserId = request.DriverId;
            if (string.IsNullOrEmpty(driverUserId))
                return Response<List<PendingBookingForDriverDto>>.Fail("غير مصرح");

            var bookings = await _unitOfWork.BookingRepository.GetAllQueryable()
                .Where(b =>
                    b.RequestStatus == RequestStatus.Pending &&
                    b.Trip.DriverId == driverUserId)
                .Include(b => b.User)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.FromCity)
                        .ThenInclude(c => c.Governorate)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.ToCity)
                        .ThenInclude(c => c.Governorate)
                .Select(b => new PendingBookingForDriverDto
                {
                    BookingId = b.Id,
                    TripId = b.TripId,
                    PassengerId = b.UserId,
                    PassengerName = b.User.FullName,
                    PassengerPicture = b.User.Picture,
                    FromCity = b.Trip.FromCity.Name,
                    FromGovernorate = b.Trip.FromCity.Governorate.Name,
                    ToCity = b.Trip.ToCity.Name,
                    ToGovernorate = b.Trip.ToCity.Governorate.Name,
                    TripDate = b.Trip.TripDate,
                    RequestDate = b.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Response<List<PendingBookingForDriverDto>>.Success(bookings);
        }
    }
}
