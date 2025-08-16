using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripOwner
{
    public class ValidateTripOwnerCommandHandler : IRequestHandler<ValidateTripOwnerCommand, Response<bool>>
    {
        public Task<Response<bool>> Handle(ValidateTripOwnerCommand request, CancellationToken cancellationToken)
        {
            if (request.CurrentUserId != request.DriverId)
                return Task.FromResult(Response<bool>.Fail("أنت غير مخول للقيام بهذا الإجراء."));

            return Task.FromResult(Response<bool>.Success(true));
        }
    }

}
