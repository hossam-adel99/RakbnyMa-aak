using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trips.Queries.GetAllTrips
{
    public record GetAllTripsQuery(int Page = 1, int PageSize = 10) : IRequest<Response<PaginatedResult<TripResponseDto>>>;

}
