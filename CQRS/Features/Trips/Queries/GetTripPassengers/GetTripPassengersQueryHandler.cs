using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;
using System.Security.Claims;

namespace RakbnyMa_aak.CQRS.Features.Trips.Queries.GetTripPassengers
{
    public class GetTripPassengersQueryHandler : IRequestHandler<GetTripPassengersQuery, Response<TripPassengersDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTripPassengersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<TripPassengersDto>> Handle(GetTripPassengersQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = request.DriverUserId;

            var trip = await _unitOfWork.TripRepository
                .GetAllQueryable()
                .Include(t => t.Driver)
                .Include(t => t.Bookings.Where(b => b.RequestStatus == RequestStatus.Confirmed))
                    .ThenInclude(b => b.User)
                        .ThenInclude(u => u.RatingsGiven)
                .FirstOrDefaultAsync(t =>
                    t.Id == request.TripId &&
                    !t.IsDeleted &&
                    (t.TripStatus == TripStatus.Ongoing || t.TripStatus == TripStatus.Completed)
                );

            if (trip == null)
                return Response<TripPassengersDto>.Fail("لم يتم العثور على الرحلة أو أن حالتها غير صالحة", statusCode: 404);

            if (trip.DriverId != currentUserId)
                return Response<TripPassengersDto>.Fail("أنت لست مالك هذه الرحلة", statusCode: 403);

            var approvedBookings = trip.Bookings.ToList();

            var passengers = approvedBookings.Select(b =>
            {
                var rating = b.User.RatingsGiven
                    .Where(r => r.RatedId == trip.Driver.UserId && r.TripId == trip.Id)
                    .Select(r => r.RatingValue)
                    .FirstOrDefault();

                return new PassengerInTripDto
                {
                    PassengerName = b.User.FullName,
                    ProfilePicture = b.User.Picture,
                    Rating = rating
                };
            }).ToList();

            var validRatings = passengers.Where(p => p.Rating > 0).Select(p => p.Rating);
            var avgRating = validRatings.Any() ? Math.Round(validRatings.Average(), 1) : 0.0;
            var totalPassengers = approvedBookings.Sum(b => b.NumberOfSeats);

            var result = new TripPassengersDto
            {
                PickupLocation = trip.PickupLocation,
                TripDate = trip.TripDate,
                TotalPassengers = totalPassengers,
                AveragePassengerRating = avgRating,
                Passengers = passengers
            };

            return Response<TripPassengersDto>.Success(result);
        }
    }
}
