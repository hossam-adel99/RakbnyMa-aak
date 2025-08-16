using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripExists;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripIsUpdatable;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripOwner;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using System.Security.Claims;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Trips.Delete_Trip
{
    public class DeleteTripCommandHandler : IRequestHandler<DeleteTripCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeleteTripCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Response<string>> Handle(DeleteTripCommand request, CancellationToken cancellationToken)
        {
            var tripResult = await _mediator.Send(new ValidateTripExistsCommand(request.TripId));
            if (!tripResult.IsSucceeded)
                return Response<string>.Fail(tripResult.Message);

            var trip = tripResult.Data;

            var isOwner = await _mediator.Send(new ValidateTripOwnerCommand(request.CurrentUserId, trip.DriverId));
            if (!isOwner.IsSucceeded)
                return Response<string>.Fail(isOwner.Message);

            var hasConfirmedBookings = await _unitOfWork.BookingRepository
                .GetAllQueryable()
                .AnyAsync(b => b.TripId == trip.TripId && b.RequestStatus == RequestStatus.Confirmed, cancellationToken);

            if (hasConfirmedBookings)
            {
                return Response<string>.Fail("لا يمكن حذف الرحلة لوجود حجوزات مؤكدة.");
            }

            trip.IsDeleted = true;
            await _unitOfWork.CompleteAsync();

            return Response<string>.Success($"تم حذف الرحلة ذات المعرف {trip.TripId} بنجاح.");
        }
    }
}
