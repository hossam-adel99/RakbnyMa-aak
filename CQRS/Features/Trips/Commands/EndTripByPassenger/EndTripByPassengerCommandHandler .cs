using MediatR;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateBookingForEnding;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.MarkBookingAsEnded;
using RakbnyMa_aak.GeneralResponse;
namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.EndTripByPassenger
{
    public class EndTripByPassengerCommandHandler : IRequestHandler<EndTripByPassengerCommand, Response<bool>>
    {
        private readonly IMediator _mediator;

        public EndTripByPassengerCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Response<bool>> Handle(EndTripByPassengerCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate booking
            var validationResponse = await _mediator.Send(
                new ValidateBookingForEndingCommand(request.BookingId, request.CurrentUserId)
            );

            if (!validationResponse.IsSucceeded)
                return Response<bool>.Fail(validationResponse.Message);

            // 2. Mark booking as ended
            return await _mediator.Send(
                new MarkBookingAsEndedCommand(validationResponse.Data.Id)
            );
        }
    }
}
