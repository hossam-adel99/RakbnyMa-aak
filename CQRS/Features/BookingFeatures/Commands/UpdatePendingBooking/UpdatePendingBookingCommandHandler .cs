using MediatR;
using RakbnyMa_aak.DTOs.BookingsDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.UpdatePendingBooking
{
    public class UpdatePendingBookingCommandHandler : IRequestHandler<UpdatePendingBookingCommand, Response<UpdateBookingSeatsResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePendingBookingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<UpdateBookingSeatsResponseDto>> Handle(UpdatePendingBookingCommand request, CancellationToken cancellationToken)
        {
            var dto = request.BookingDto;
            var difference = dto.NewNumberOfSeats - request.OldSeats;

            if (difference < 0)
                return Response<UpdateBookingSeatsResponseDto>.Fail("لا يمكن تقليل عدد المقاعد قبل الموافقة");

            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(dto.BookingId);
            if (booking is null || booking.UserId != request.userId)
                return Response<UpdateBookingSeatsResponseDto>.Fail("لم يتم العثور على الحجز أو لا تملك صلاحية الوصول");

            booking.NumberOfSeats = dto.NewNumberOfSeats;
            booking.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.BookingRepository.Update(booking);
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
