using MediatR;
using RakbnyMa_aak.DTOs.BookingsDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Queries.GetUserBookings
{
    public class GetMyBookingsQuery : IRequest<Response<PaginatedResult<UserBookingResponseDto>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public RequestStatus? Status { get; set; }
        public string userId { get; set; }

        public GetMyBookingsQuery(int page, int pageSize, RequestStatus? status , string userId)
        {
            Page = page;
            PageSize = pageSize;
            Status = status;
            this.userId = userId;
        }
    }

}
