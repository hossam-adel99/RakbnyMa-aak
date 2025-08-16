using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Queries.GetTripCoordinates
{
    public class GetTripCoordinatesQuery : IRequest<Response<TripCoordinatesResponseDto>>
    {
        public int TripId { get; set; }

        public GetTripCoordinatesQuery(int tripId)
        {
            TripId = tripId;
        }
    }

}
