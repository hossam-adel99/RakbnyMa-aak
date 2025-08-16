using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Ratings.UserAddRating
{
    public class UserAddRatingCommandHandler : IRequestHandler<UserAddRatingCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAddRatingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(UserAddRatingCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RatingDto;

            // Step 1: Check trip
            var trip = await _unitOfWork.TripRepository.GetByIdAsync(dto.TripId);
            if (trip == null || trip.IsDeleted)
                return Response<bool>.Fail("الرحلة غير موجودة.");

            // Step 2: Check booking for this user and trip
            var booking = await _unitOfWork.BookingRepository
                .GetConfirmedFinishedBookingQueryable(dto.TripId, dto.RaterId)
                .FirstOrDefaultAsync();

            if (booking == null)
                return Response<bool>.Fail("لا يمكنك تقييم هذه الرحلة إلا بعد الانتهاء منها.");

            // Step 3: Check if already rated
            bool alreadyRated = await _unitOfWork.RatingRepository
                .AnyAsync(r => r.TripId == dto.TripId &&
                               r.RaterId == dto.RaterId &&
                               r.RatedId == trip.DriverId);

            if (alreadyRated)
                return Response<bool>.Fail("لقد قمت بتقييم هذا السائق لهذه الرحلة مسبقاً.");

            // Step 4: Add rating
            var rating = new Rating
            {
                TripId = dto.TripId,
                RaterId = dto.RaterId,
                RatedId = trip.DriverId,
                RatingValue = dto.RatingValue,
                Comment = dto.Comment,
            };

            await _unitOfWork.RatingRepository.AddAsync(rating);
            await _unitOfWork.CompleteAsync();

            return Response<bool>.Success(true, "تم تقييم السائق بنجاح.");
        }
    }
}
