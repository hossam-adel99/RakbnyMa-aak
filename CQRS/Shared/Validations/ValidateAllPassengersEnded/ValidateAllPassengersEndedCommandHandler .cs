using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateAllPassengersEnded
{
    public class ValidateAllPassengersEndedCommandHandler : IRequestHandler<ValidateAllPassengersEndedCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidateAllPassengersEndedCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(ValidateAllPassengersEndedCommand request, CancellationToken cancellationToken)
        {
            var anyActive = await _unitOfWork.BookingRepository
                .GetBookingsByTripIdQueryable(request.TripId)
                .AnyAsync(b => !b.HasEnded);

            if (anyActive)
                return Response<bool>.Fail("يجب أن يُنهي جميع الركاب الرحلة أولاً.", false);

            return Response<bool>.Success(true, "جميع الركاب أنهوا الرحلة.");
        }
    }
}
