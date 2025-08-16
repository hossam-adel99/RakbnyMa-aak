using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Ratings.UserUpdateRating
{
    public record UserUpdateRatingCommand(UserUpdateRatingDto RatingDto) : IRequest<Response<bool>>;
}
