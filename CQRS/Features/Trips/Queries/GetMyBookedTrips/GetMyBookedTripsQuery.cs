using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trips.Queries.GetMyBookedTrips
{
    public class GetMyBookedTripsQuery : IRequest<Response<PaginatedResult<TripResponseDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetMyBookedTripsQuery(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}
