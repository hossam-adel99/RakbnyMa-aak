using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Ratings.DriverAddRating
{
    public class DriverAddRatingCommandHandler : IRequestHandler<DriverAddRatingCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DriverAddRatingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(DriverAddRatingCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RatingDto;

            // الخطوة 1: التحقق من الرحلة
            var trip = await _unitOfWork.TripRepository.GetByIdAsync(dto.TripId);
            if (trip == null || trip.IsDeleted)
                return Response<bool>.Fail("لم يتم العثور على الرحلة.");

            // الخطوة 2: التحقق من أن السائق هو صاحب الرحلة
            if (trip.DriverId != dto.RaterId)
                return Response<bool>.Fail("غير مسموح لك بتقييم الركاب في هذه الرحلة.");

            if (dto.RaterId == dto.RatedPassengerId)
                return Response<bool>.Fail("لا يمكنك تقييم نفسك.");

            // الخطوة 3: التحقق من أن الراكب أكمل الرحلة
            var booking = await _unitOfWork.BookingRepository
                .GetConfirmedFinishedBookingQueryable(dto.TripId, dto.RatedPassengerId)
                .FirstOrDefaultAsync();

            if (booking == null)
                return Response<bool>.Fail("هذا الراكب لم يُكمل هذه الرحلة.");

            // الخطوة 4: التحقق من أنه تم التقييم مسبقاً
            bool alreadyRated = await _unitOfWork.RatingRepository
                .AnyAsync(r => r.TripId == dto.TripId &&
                               r.RaterId == dto.RaterId &&
                               r.RatedId == dto.RatedPassengerId);

            if (alreadyRated)
                return Response<bool>.Fail("لقد قمت بالفعل بتقييم هذا الراكب لهذه الرحلة.");

            // الخطوة 5: إضافة التقييم
            var rating = new Rating
            {
                TripId = dto.TripId,
                RaterId = dto.RaterId,
                RatedId = dto.RatedPassengerId,
                RatingValue = dto.RatingValue,
                Comment = dto.Comment,
            };

            await _unitOfWork.RatingRepository.AddAsync(rating);
            await _unitOfWork.CompleteAsync();

            return Response<bool>.Success(true, "تم تقييم الراكب بنجاح.");
        }
    }
}
