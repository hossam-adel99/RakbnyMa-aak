using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Ratings.UserDeleteRating
{
    public class UserDeleteRatingCommandHandler : IRequestHandler<UserDeleteRatingCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserDeleteRatingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(UserDeleteRatingCommand request, CancellationToken cancellationToken)
        {
            var rating = await _unitOfWork.RatingRepository.GetByIdAsync(request.RatingId);

            if (rating == null)
                return Response<bool>.Fail("لم يتم العثور على التقييم.");

            if (rating.RaterId != request.RaterId)
                return Response<bool>.Fail("غير مصرح لك: يمكنك فقط حذف تقييماتك الخاصة.");

            _unitOfWork.RatingRepository.Delete(rating);
            await _unitOfWork.CompleteAsync();

            return Response<bool>.Success(true, "تم حذف التقييم بنجاح.");
        }
    }
}
