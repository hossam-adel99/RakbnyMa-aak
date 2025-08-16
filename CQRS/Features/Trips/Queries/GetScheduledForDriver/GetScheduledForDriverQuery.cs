using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.RequestsDTOs;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trips.Queries.GetScheduledForDriver
{
    public class GetScheduledForDriverQuery : IRequest<Response<PaginatedResult<TripResponseDto>>>
    {
        public ScheduledTripRequestDto Filter { get; set; } = new();
        public string DriverId { get; set; } = string.Empty;

        public GetScheduledForDriverQuery() { }

        public GetScheduledForDriverQuery(ScheduledTripRequestDto filter, string driverId)
        {
            Filter = filter;
            DriverId = driverId;
        }
    }
}
