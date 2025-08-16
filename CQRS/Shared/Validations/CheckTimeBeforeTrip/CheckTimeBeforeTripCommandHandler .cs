using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Commands.Validations.CheckTimeBeforeTrip
{
    public class CheckTimeBeforeTripCommandHandler : IRequestHandler<CheckTimeBeforeTripCommand, Response<bool>>
    {
        public Task<Response<bool>> Handle(CheckTimeBeforeTripCommand request, CancellationToken cancellationToken)
        {
            var isAllowed = request.StartDateTime > DateTime.UtcNow.AddHours(request.MinimumHours);
            return Task.FromResult(isAllowed
                ? Response<bool>.Success(true)
                : Response<bool>.Fail($"يجب الإلغاء قبل موعد الرحلة على الأقل بـ {request.MinimumHours} ساعة."));
        }
    }

}
