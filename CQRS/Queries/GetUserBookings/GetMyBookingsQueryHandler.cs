using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.BookingsDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using System.Security.Claims;

namespace RakbnyMa_aak.CQRS.Queries.GetUserBookings
{
    public class GetMyBookingsQueryHandler : IRequestHandler<GetMyBookingsQuery, Response<PaginatedResult<UserBookingResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyBookingsQueryHandler(IUnitOfWork unitOfWork)//, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
           // _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<PaginatedResult<UserBookingResponseDto>>> Handle(GetMyBookingsQuery request, CancellationToken cancellationToken)
        {
            //var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (string.IsNullOrEmpty(userId))
            //    return Response<PaginatedResult<UserBookingResponseDto>>.Fail("Unauthorized");

            var userId = request.userId;

            var bookingsQuery = _unitOfWork.BookingRepository.GetAllQueryable()
                .Include(b => b.Trip)
                    .ThenInclude(t => t.FromCity)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.ToCity)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.Driver)
                        .ThenInclude(d => d.User)
                .Include(b => b.Trip)
                    .ThenInclude(t => t.Driver)
                        .ThenInclude(d => d.User.RatingsReceived)
                .Where(b => b.UserId == userId && !b.Trip.IsDeleted);

            if (request.Status.HasValue)
                bookingsQuery = bookingsQuery.Where(b => b.RequestStatus == request.Status.Value);

            var totalCount = await bookingsQuery.CountAsync(cancellationToken);

            var bookings = await bookingsQuery
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);


            var result = bookings.Select(b => new UserBookingResponseDto
            {
                BookingId = b.Id,
                TripId = b.TripId,
                DriverFullName = b.Trip.Driver.User.FullName,
                DriverPicture = b.Trip.Driver.User.Picture,
                DriverRate = b.Trip.Driver.User.RatingsReceived.Any() ? b.Trip.Driver.User.RatingsReceived.Average(r => r.RatingValue).ToString("0.0") : "0.0",
                FromCityName = b.Trip.FromCity.Name,
                ToCityName = b.Trip.ToCity.Name,
                TripDate = b.Trip.TripDate,
                BookingStatus = b.RequestStatus,
                NumberOfSeats = b.NumberOfSeats,
                TotalPrice = b.TotalPrice
            }).ToList();


            return Response<PaginatedResult<UserBookingResponseDto>>.Success(
                new PaginatedResult<UserBookingResponseDto>(
                    result,
                    totalCount,
                    request.Page,
                    request.PageSize
                )
            );
        }
    }

}
