using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Ratings.DriverUpdateRating
{
    public class DriverUpdateRatingCommandHandler : IRequestHandler<DriverUpdateRatingCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DriverUpdateRatingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(DriverUpdateRatingCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RatingDto;

            var rating = await _unitOfWork.RatingRepository.GetByIdAsync(dto.RatingId);
            if (rating == null)
                return Response<bool>.Fail("لم يتم العثور على التقييم.");

            if (rating.RaterId != dto.RaterId)
                return Response<bool>.Fail("غير مصرح لك: يمكنك تعديل تقييماتك فقط.");

            if (dto.RatingValue.HasValue)
                rating.RatingValue = dto.RatingValue.Value;

            if (!string.IsNullOrWhiteSpace(dto.Comment))
                rating.Comment = dto.Comment;

            _unitOfWork.RatingRepository.Update(rating);
            await _unitOfWork.CompleteAsync();

            return Response<bool>.Success(true, "تم تعديل التقييم بنجاح.");
        }
    }
}
