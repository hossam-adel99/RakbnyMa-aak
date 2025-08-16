using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Ratings.DriverGetRatings
{
    public record GetDriverRatingsQuery(string DriverId, int Page = 1, int PageSize = 10)
        : IRequest<Response<PaginatedResult<DriverRatingDto>>>;
}
