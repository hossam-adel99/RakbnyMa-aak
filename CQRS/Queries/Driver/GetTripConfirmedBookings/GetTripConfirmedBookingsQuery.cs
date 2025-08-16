using MediatR;
using RakbnyMa_aak.DTOs.DriverDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Queries.Driver.GetTripConfirmedBookings
{
    public record GetTripConfirmedBookingsQuery(int TripId, int Page = 1, int PageSize = 10)
      : IRequest<Response<PaginatedResult<BookingStatusResponseDto>>>;

}
