using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Ratings.DriverDeleteRating
{
    public class DriverDeleteRatingCommandHandler : IRequestHandler<DriverDeleteRatingCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DriverDeleteRatingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(DriverDeleteRatingCommand request, CancellationToken cancellationToken)
        {
            var rating = await _unitOfWork.RatingRepository.GetByIdAsync(request.RatingId);

            if (rating == null)
                return Response<bool>.Fail("لم يتم العثور على التقييم.");

            if (rating.RaterId != request.RaterId)
                return Response<bool>.Fail("غير مصرح لك: يمكنك حذف تقييماتك فقط.");

            _unitOfWork.RatingRepository.Delete(rating);
            await _unitOfWork.CompleteAsync();

            return Response<bool>.Success(true, "تم حذف التقييم بنجاح.");
        }
    }
}
