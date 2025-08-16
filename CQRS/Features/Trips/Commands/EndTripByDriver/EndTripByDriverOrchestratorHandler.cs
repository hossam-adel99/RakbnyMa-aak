using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripForEnding;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateAllPassengersEnded;
using RakbnyMa_aak.CQRS.Features.Trip.Commands.CompleteTrip;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.EndTripByDriver
{
    public class EndTripByDriverOrchestratorHandler : IRequestHandler<EndTripByDriverOrchestrator, Response<bool>>
    {
        private readonly IMediator _mediator;

        public EndTripByDriverOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Response<bool>> Handle(EndTripByDriverOrchestrator request, CancellationToken cancellationToken)
        {
            // 1. Validate trip
            var tripValidation = await _mediator.Send(
                new ValidateTripForEndingCommand(request.TripId, request.DriverId)
            );

            if (!tripValidation.IsSucceeded)
                return Response<bool>.Fail(tripValidation.Message);

            // 2. Validate passengers
            var passengersValidation = await _mediator.Send(
                new ValidateAllPassengersEndedCommand(request.TripId)
            );

            if (!passengersValidation.IsSucceeded)
                return Response<bool>.Fail(passengersValidation.Message);

            // 3. Complete trip
            var completeTripResult = await _mediator.Send(
                new CompleteTripCommand(request.TripId)
            );

            return completeTripResult;
        }
    }
}
