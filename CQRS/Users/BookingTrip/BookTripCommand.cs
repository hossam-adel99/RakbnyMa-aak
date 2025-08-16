using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Users.BookingTrip
{
    public record BookTripCommand(BookTripDto TripDetails) : IRequest<Response<int>>;
}
