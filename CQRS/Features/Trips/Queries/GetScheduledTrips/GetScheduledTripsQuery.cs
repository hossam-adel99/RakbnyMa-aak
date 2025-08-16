using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.RequestsDTOs;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trips.Queries.GetScheduledTrips
{
    public class GetScheduledTripsQuery : IRequest<Response<PaginatedResult<TripResponseDto>>>
    {
        public ScheduledTripRequestDto Filter { get; }

        public GetScheduledTripsQuery(ScheduledTripRequestDto filter)
        {
            Filter = filter;
        }
    }
}
