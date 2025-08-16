using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Ratings.CommonAddRating
{
    public class AddRatingOrchestratorHandler : IRequestHandler<AddRatingOrchestrator, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddRatingOrchestratorHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(AddRatingOrchestrator request, CancellationToken cancellationToken)
        {
            var dto = request.RatingDto;

            // الخطوة 1: التحقق من الرحلة
            var trip = await _unitOfWork.TripRepository.GetByIdAsync(dto.TripId);
            if (trip == null || trip.IsDeleted)
                return Response<bool>.Fail("لم يتم العثور على الرحلة.");

            // الخطوة 2: التحقق من الصلاحية (السائق أو الراكب)
            if (dto.IsDriverRating)
            {
                if (trip.DriverId != dto.RaterId)
                    return Response<bool>.Fail("السائق غير مخول بهذه الرحلة.");

                var booking = await _unitOfWork.BookingRepository
                    .GetConfirmedFinishedBookingQueryable(dto.TripId, dto.RatedId)
                    .FirstOrDefaultAsync();

                if (booking == null)
                    return Response<bool>.Fail("هذا الراكب لم يُكمل هذه الرحلة.");
            }
            else
            {
                var booking = await _unitOfWork.BookingRepository
                    .GetConfirmedFinishedBookingQueryable(dto.TripId, dto.RaterId)
                    .FirstOrDefaultAsync();

                if (booking == null)
                    return Response<bool>.Fail("يمكنك تقييم هذه الرحلة فقط بعد إكمالها.");
            }

            // الخطوة 3: منع التقييم الذاتي
            if (dto.RaterId == dto.RatedId)
                return Response<bool>.Fail("لا يمكنك تقييم نفسك.");

            // الخطوة 4: التحقق من وجود تقييم سابق
            bool alreadyRated = await _unitOfWork.RatingRepository
                .AnyAsync(r => r.TripId == dto.TripId &&
                               r.RaterId == dto.RaterId &&
                               r.RatedId == dto.RatedId);

            if (alreadyRated)
                return Response<bool>.Fail("لقد قمت بتقييم هذا الشخص مسبقًا لهذه الرحلة.");

            // الخطوة 5: إضافة التقييم
            var rating = new Rating
            {
                TripId = dto.TripId,
                RaterId = dto.RaterId,
                RatedId = dto.RatedId,
                RatingValue = dto.RatingValue,
                Comment = dto.Comment
            };

            await _unitOfWork.RatingRepository.AddAsync(rating);
            await _unitOfWork.CompleteAsync();

            return Response<bool>.Success(true, "تمت إضافة التقييم بنجاح.");
        }
    }

}
