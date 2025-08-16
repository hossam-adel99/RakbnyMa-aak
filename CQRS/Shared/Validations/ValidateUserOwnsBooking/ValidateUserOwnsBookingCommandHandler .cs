using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateUserOwnsBooking
{
    public class ValidateUserOwnsBookingCommandHandler : IRequestHandler<ValidateUserOwnsBookingCommand, Response<ValidateBookingOwnerShipResultDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidateUserOwnsBookingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<ValidateBookingOwnerShipResultDto>> Handle(ValidateUserOwnsBookingCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.BookingRepository
                .GetAllQueryable()
                .Where(b => b.Id == request.BookingId && !b.IsDeleted)
                .Where(b => b.UserId == request.UserId)
                .Select(b => new ValidateBookingOwnerShipResultDto
                {
                    TripId = b.TripId,
                    NumberOfSeats = b.NumberOfSeats,
                    RequestStatus = b.RequestStatus
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
                return Response<ValidateBookingOwnerShipResultDto>.Fail("لم يتم العثور على الحجز أو ليس لديك صلاحية الوصول.");

            return Response<ValidateBookingOwnerShipResultDto>.Success(result);
        }
    }

}
