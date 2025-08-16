using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.DriverDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;
using System.Linq.Expressions;
using System.Security.Claims;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Queries.Driver.GetPendingBooking
{
    public class GetTripPendingBookingsQueryHandler
         : IRequestHandler<GetTripPendingBookingsQuery, Response<PaginatedResult<BookingStatusResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTripPendingBookingsQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<PaginatedResult<BookingStatusResponseDto>>> Handle(
            GetTripPendingBookingsQuery request,
            CancellationToken cancellationToken)
        {
            var driverId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(driverId))
                return Response<PaginatedResult<BookingStatusResponseDto>>.Fail("غير مصرح");


            var trip = await _unitOfWork.TripRepository.GetByIdAsync(request.TripId);
            if (trip is null || trip.IsDeleted)
                return Response<PaginatedResult<BookingStatusResponseDto>>.Fail("الرحلة غير موجودة");

            if (trip.DriverId != driverId)
                return Response<PaginatedResult<BookingStatusResponseDto>>.Fail("أنت لست مالك هذه الرحلة");

            Expression<Func<Booking, bool>> filter = b =>
                b.TripId == request.TripId &&
                b.RequestStatus == Utilities.Enums.RequestStatus.Pending;


            var result = await _unitOfWork.BookingRepository.GetProjectedPaginatedAsync<BookingStatusResponseDto>(
                predicate: filter,
                page: request.Page,
                pageSize: request.PageSize,
                mapper: _mapper,
                cancellationToken: cancellationToken);

            return Response<PaginatedResult<BookingStatusResponseDto>>.Success(result);
        }
    }
}
