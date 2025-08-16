using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.CQRS.Queries.GetUserRatings;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Ratings.UserGetRatings
{
    public class GetUserRatingsQueryHandler : IRequestHandler<GetUserRatingsQuery, Response<PaginatedResult<UserRatingDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserRatingsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<PaginatedResult<UserRatingDto>>> Handle(GetUserRatingsQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Filter;

            var query = _unitOfWork.RatingRepository
                .GetAllQueryable()
                .Where(r => r.RaterId == filter.RaterId);

            if (filter.StarsFilter.HasValue)
                query = query.Where(r => r.RatingValue == filter.StarsFilter.Value);

            if (filter.HasComment.HasValue)
            {
                if (filter.HasComment.Value)
                    query = query.Where(r => !string.IsNullOrEmpty(r.Comment));
                else
                    query = query.Where(r => string.IsNullOrEmpty(r.Comment));
            }

            int totalCount = await query.CountAsync(cancellationToken);

            var ratings = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(r => new UserRatingDto
                {
                    RatingId = r.Id,
                    TripId = r.TripId,
                    DriverName = r.Rated.FullName,
                    RatingValue = r.RatingValue,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<UserRatingDto>(ratings, totalCount, filter.Page, filter.PageSize);

            return Response<PaginatedResult<UserRatingDto>>.Success(result);
        }
    }
}
