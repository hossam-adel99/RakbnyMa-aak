using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.CompleteTrip
{
    public class CompleteTripCommandHandler : IRequestHandler<CompleteTripCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompleteTripCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(CompleteTripCommand request, CancellationToken cancellationToken)
        {
            var trip = await _unitOfWork.TripRepository.GetByIdAsync(request.TripId);

            if (trip == null || trip.IsDeleted)
                return Response<bool>.Fail("لم يتم العثور على الرحلة أو تم حذفها.");

            trip.TripStatus = TripStatus.Completed;
            trip.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.TripRepository.Update(trip);
            await _unitOfWork.CompleteAsync();

            return Response<bool>.Success(true, "تم إنهاء الرحلة بنجاح");
        }
    }

}
