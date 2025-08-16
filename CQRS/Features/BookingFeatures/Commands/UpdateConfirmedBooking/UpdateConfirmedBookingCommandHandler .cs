using MediatR;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.DecreaseBookingSeats;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.IncreaseBookingSeats;
using RakbnyMa_aak.CQRS.Features.Trip.Commands.DecreaseTripSeats;
using RakbnyMa_aak.CQRS.Features.Trip.Commands.IncreaseTripSeats;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.DTOs.BookingsDTOs.ResponseDTOs;
using RakbnyMa_aak.DTOs.Shared;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.UpdateConfirmedBooking
{
    public class UpdateConfirmedBookingCommandHandler : IRequestHandler<UpdateConfirmedBookingCommand, Response<UpdateBookingSeatsResponseDto>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _dbContext;

        public UpdateConfirmedBookingCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, AppDbContext dbContext)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task<Response<UpdateBookingSeatsResponseDto>> Handle(UpdateConfirmedBookingCommand request, CancellationToken cancellationToken)
        {
            var dto = request.BookingDto;
            var difference = dto.NewNumberOfSeats - request.OldSeats;

            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(dto.BookingId);
            if (booking is null || booking.UserId != request.userId)
                return Response<UpdateBookingSeatsResponseDto>.Fail("لم يتم العثور على الحجز أو ليس لديك صلاحية الوصول إليه");

            if (difference > 0)
            {
                var bookingResult = await _mediator.Send(new IncreaseBookingSeatsCommand(new UpdateBookingSeatsRequestDto
                {
                    BookingId = dto.BookingId,
                    SeatsToChange = difference
                }));
                if (!bookingResult.IsSucceeded)
                    return Response<UpdateBookingSeatsResponseDto>.Fail($"فشل في زيادة عدد مقاعد الحجز: {bookingResult.Message}");

                var tripResult = await _mediator.Send(new DecreaseTripSeatsCommand(new DecreaseTripSeatsDto
                {
                    TripId = dto.TripId,
                    NumberOfSeats = difference
                }));
                if (!tripResult.IsSucceeded)
                    return Response<UpdateBookingSeatsResponseDto>.Fail($"فشل في تقليل عدد مقاعد الرحلة: {tripResult.Message}");
            }
            else
            {
                var bookingResult = await _mediator.Send(new DecreaseBookingSeatsCommand(new UpdateBookingSeatsRequestDto
                {
                    BookingId = dto.BookingId,
                    SeatsToChange = difference
                }));
                if (!bookingResult.IsSucceeded)
                    return Response<UpdateBookingSeatsResponseDto>.Fail($"فشل في تقليل عدد مقاعد الحجز: {bookingResult.Message}");

                var tripResult = await _mediator.Send(new IncreaseTripSeatsCommand(new IncreaseTripSeatsDto
                {
                    TripId = dto.TripId,
                    NumberOfSeats = -difference
                }));
                if (!tripResult.IsSucceeded)
                    return Response<UpdateBookingSeatsResponseDto>.Fail($"فشل في زيادة عدد مقاعد الرحلة: {tripResult.Message}");
            }

            await _unitOfWork.CompleteAsync();
            return Response<UpdateBookingSeatsResponseDto>.Success(new UpdateBookingSeatsResponseDto
            {
                BookingId = dto.BookingId,
                OldSeats = request.OldSeats,
                NewSeats = dto.NewNumberOfSeats,
                UpdatedAt = DateTime.UtcNow
            }, "تم تحديث الحجز بنجاح");
        }
    }
}
