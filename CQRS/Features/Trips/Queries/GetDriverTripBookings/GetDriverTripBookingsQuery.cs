using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trip.Queries.GetDriverTripBookings
{
    public record GetDriverTripBookingsQuery(string DriverUserId) : IRequest<Response<List<BookingForDriverDto>>>;
}
