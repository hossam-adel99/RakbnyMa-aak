using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Queries.GetUserRatings
{
    public record GetUserRatingsQuery(GetUserRatingDto Filter)
        : IRequest<Response<PaginatedResult<UserRatingDto>>>;
}
