using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Ratings.DriverUpdateRating
{
    public record DriverUpdateRatingCommand(DriverUpdateRatingDto RatingDto)
        : IRequest<Response<bool>>;
}
