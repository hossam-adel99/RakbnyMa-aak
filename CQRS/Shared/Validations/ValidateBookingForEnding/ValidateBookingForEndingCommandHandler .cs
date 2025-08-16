using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateBookingForEnding
{
    public class ValidateBookingForEndingCommandHandler : IRequestHandler<ValidateBookingForEndingCommand, Response<ValidateBookingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidateBookingForEndingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<ValidateBookingDto>> Handle(ValidateBookingForEndingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.BookingRepository.GetAllQueryable()
                 .Where(b => b.Id == request.BookingId)
                .Select(b => new ValidateBookingDto
                {
                    Id = b.Id,
                    HasStarted = b.HasStarted,
                    UserId = b.UserId,
                    IsDeleted = b.IsDeleted
                })
                 .FirstOrDefaultAsync(cancellationToken);

            if (booking == null || booking.IsDeleted || booking.UserId != request.CurrentUserId)
                return Response<ValidateBookingDto>.Fail("غير مصرح أو لم يتم العثور على الحجز.");

            if (!booking.HasStarted)
                return Response<ValidateBookingDto>.Fail("يجب بدء الرحلة أولاً.");

            return Response<ValidateBookingDto>.Success(booking, "الحجز صالح لإنهاء الرحلة.");
        }
    }
}
