using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trips.Queries.GetTripByBookingId
{
        public record GetTripByBookingIdQuery(int BookingId, string userId) : IRequest<Response<TripResponseDto>>;
}
