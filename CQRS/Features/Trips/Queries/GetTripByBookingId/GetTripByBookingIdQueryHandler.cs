using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using System.Security.Claims;

namespace RakbnyMa_aak.CQRS.Features.Trips.Queries.GetTripByBookingId
{
    public class GetTripByBookingIdHandler : IRequestHandler<GetTripByBookingIdQuery, Response<TripResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTripByBookingIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<TripResponseDto>> Handle(GetTripByBookingIdQuery request, CancellationToken cancellationToken)
        {
            var userId = request.userId;
            if (string.IsNullOrEmpty(userId))
                return Response<TripResponseDto>.Fail("غير مصرح");

            var booking = await _unitOfWork.BookingRepository
                .GetAllQueryable()
                .Include(b => b.Trip)
                    .ThenInclude(t => t.Driver)
                        .ThenInclude(d => d.User)
                            .ThenInclude(u => u.RatingsReceived)
                .Include(b => b.Trip.FromCity)
                    .ThenInclude(c => c.Governorate)
                .Include(b => b.Trip.ToCity)
                    .ThenInclude(c => c.Governorate)
                .FirstOrDefaultAsync(b => b.Id == request.BookingId && !b.IsDeleted, cancellationToken);

            if (booking is null)
                return Response<TripResponseDto>.Fail("لم يتم العثور على الحجز أو لا يسمح بالوصول");

            var dto = _mapper.Map<TripResponseDto>(booking.Trip);
            return Response<TripResponseDto>.Success(dto);
        }
    }
}
