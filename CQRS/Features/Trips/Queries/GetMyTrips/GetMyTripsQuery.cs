using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trip.Queries.GetMyTrips
{
    public record GetMyTripsQuery(int Page = 1, int PageSize = 10)
     : IRequest<Response<PaginatedResult<TripResponseDto>>>;

}
