using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Queries.Driver.GetPendingBookingsForDriver
{
    public class GetPendingBookingsForDriverQuery : IRequest<Response<List<PendingBookingForDriverDto>>>
    {
        public string DriverId { get; set; }
        public GetPendingBookingsForDriverQuery(string driverId)
        {
            DriverId = driverId;
        }
    }
}