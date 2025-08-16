using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateUserOwnsBooking;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateOwnershipAndGetBooking
{
    public class ValidateOwnershipAndGetBookingHandler : IRequestHandler<ValidateOwnershipAndGetBookingCommand, Response<ValidateBookingOwnerShipResultDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidateOwnershipAndGetBookingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<ValidateBookingOwnerShipResultDto>> Handle(ValidateOwnershipAndGetBookingCommand request, CancellationToken cancellationToken)
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
                return Response<ValidateBookingOwnerShipResultDto>.Fail("لم يتم العثور على الحجز أو لا تملك صلاحية الوصول إليه.");

            return Response<ValidateBookingOwnerShipResultDto>.Success(result);
        }
    }

}
