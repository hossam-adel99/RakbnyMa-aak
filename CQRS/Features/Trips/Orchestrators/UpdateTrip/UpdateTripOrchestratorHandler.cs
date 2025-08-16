using MediatR;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripIsUpdatable;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripOwner;
using RakbnyMa_aak.CQRS.Features.Trip.Commands.UpdateTrip;
using RakbnyMa_aak.CQRS.Features.Trip.Orchestrators.ValidateTripBusinessLogic;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trip.Orchestrators.UpdateTrip
{
    public class UpdateTripOrchestratorHandler : IRequestHandler<UpdateTripOrchestrator, Response<int>>
    {
        private readonly IMediator _mediator;

        public UpdateTripOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Response<int>> Handle(UpdateTripOrchestrator request, CancellationToken cancellationToken)
        {
            request.TripDto.DriverId=request.CurrentUserId;
            var dto = request.TripDto;
            // Step 1: Validate that the trip exists and is updatable
            var tripResult = await _mediator.Send(new ValidateTripIsUpdatableCommand(request.TripId));
            if (!tripResult.IsSucceeded)
                return Response<int>.Fail(tripResult.Message);

            var trip = tripResult.Data;

            // Step 2: Validate that the current user is the owner (driver) of the trip
            var isOwner = await _mediator.Send(new ValidateTripOwnerCommand(request.CurrentUserId, trip.DriverId));
            if (!isOwner.IsSucceeded)
                return Response<int>.Fail(isOwner.Message);

            // Step 3: Validate common validations for the trip DTO
            var commonValidation = await _mediator.Send(new ValidateTripBusinessLogicOrchestrator(request.TripDto));
            if (!commonValidation.IsSucceeded)
                return Response<int>.Fail(commonValidation.Message);

            // Step 4: All validations passed, update the trip in the database

            var updateResult = await _mediator.Send(new UpdateTripCommand
            (
            request.TripId,
             dto
            ));

            return updateResult;
        }

    }

}
