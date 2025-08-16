using MediatR;
using RakbnyMa_aak.CQRS.Commands.SendNotification;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.UpdateBookingStatus;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Orchestrators.BookValidationOrchestrator;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Services.Interfaces;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.ApproveBookingRequest
{
    public class ApproveBookingOrchestratorHandler : IRequestHandler<ApproveBookingOrchestrator, Response<bool>>
    {
        private readonly IMediator _mediator;
        private readonly INotificationService _notificationService;

        public ApproveBookingOrchestratorHandler(
             IMediator mediator,
             INotificationService notificationService)
        {
            _mediator = mediator;
            _notificationService = notificationService;
        }

        public async Task<Response<bool>> Handle(ApproveBookingOrchestrator request, CancellationToken cancellationToken)
        {
            var validation = await _mediator.Send(new BookingValidationOrchestrator(request.Dto.BookingId, request.Dto.CurrentUserId));
            if (!validation.IsSucceeded)
                return Response<bool>.Fail(validation.Message);

            var result = validation.Data;

            // Step 4: Update booking status to Confirmed (internally checks available seats)
            var approveResult = await _mediator.Send(
                new UpdateBookingStatusCommand(
                    result.BookingId,
                    result.TripId,
                    RequestStatus.Confirmed));
            if (!approveResult.IsSucceeded)
                return Response<bool>.Fail(approveResult.Message);

            await _notificationService.SendNotificationAsync(
                  recipientUserId: result.PassengerId,
                  sender: new ApplicationUser
                  {
                      Id = result.DriverId,
                      FullName = result.DriverFullName,
                      Picture = result.DriverPicture
                  },
                  message: "تمت الموافقة على طلب الحجز الخاص بك.",
                  type: NotificationType.BookingApproved,
                  relatedEntityId: result.BookingId.ToString()
              );

            return Response<bool>.Success(true, "تمت الموافقة على الحجز وتم إشعار الراكب.");
        }
    }
}
