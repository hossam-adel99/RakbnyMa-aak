using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateBookingExists
{
    public class ValidateBookingExistsCommandHandler : IRequestHandler<ValidateBookingExistsCommand, Response<Booking>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidateBookingExistsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<Booking>> Handle(ValidateBookingExistsCommand request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.BookingRepository
                     .GetByIdQueryable(request.BookingId)
                     .FirstOrDefaultAsync();

            if (booking == null)
                return Response<Booking>.Fail("لم يتم العثور على الحجز.");
            if (booking.IsDeleted || booking.RequestStatus == RequestStatus.Cancelled)
                return Response<Booking>.Fail("الحجز غير صالح (محذوف أو ملغى).");

            return Response<Booking>.Success(booking);
        }
    }

}
