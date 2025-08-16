using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Ratings.DriverGetRatings
{
    public class GetDriverRatingsQueryHandler : IRequestHandler<GetDriverRatingsQuery, Response<PaginatedResult<DriverRatingDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDriverRatingsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<PaginatedResult<DriverRatingDto>>> Handle(GetDriverRatingsQuery request, CancellationToken cancellationToken)
        {
            var driverExists = await _unitOfWork.UserRepository.GetAllQueryable()
                .AnyAsync(u => u.Id == request.DriverId && u.UserType == UserType.Driver);

            if (!driverExists)
                return Response<PaginatedResult<DriverRatingDto>>.Fail("السائق غير موجود.");

            var query = _unitOfWork.RatingRepository
                .GetAllQueryable()
                .Where(r => r.RatedId == request.DriverId);

            int totalCount = await query.CountAsync(cancellationToken);

            var pagedRatings = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(r => new DriverRatingDto
                {
                    RatingId = r.Id,
                    TripId = r.TripId,
                    RatingValue = r.RatingValue,
                    Comment = r.Comment,
                    RaterName = r.Rater.FullName,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<DriverRatingDto>(pagedRatings, totalCount, request.Page, request.PageSize);

            return Response<PaginatedResult<DriverRatingDto>>.Success(result);
        }
    }
}
