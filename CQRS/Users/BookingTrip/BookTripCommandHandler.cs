using AutoMapper;
using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Users.BookingTrip
{
    public class BookTripCommandHandler : IRequestHandler<BookTripCommand, Response<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookTripCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(BookTripCommand request, CancellationToken cancellationToken)
        {
            var dto = request.TripDetails;

            var isAlreadyBooked = await _unitOfWork.BookingRepository
                .IsUserAlreadyBookedAsync(dto.PassengerUserId, dto.TripId);

            if (isAlreadyBooked)
                return Response<int>.Fail("You have already booked this trip.");

            var booking = _mapper.Map<Booking>(dto);
            booking.CreatedAt = DateTime.UtcNow;
            booking.RequestStatus = RequestStatus.Pending;
            // booking.IsPaid = dto.PaymentMethod.ToLower() == "online" ? false : true;

            await _unitOfWork.BookingRepository.AddAsync(booking);
            await _unitOfWork.CompleteAsync();

            return Response<int>.Success(booking.Id, "Trip booked successfully.");
        }
    }
}

