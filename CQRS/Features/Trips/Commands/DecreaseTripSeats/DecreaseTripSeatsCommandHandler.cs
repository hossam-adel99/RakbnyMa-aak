using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.DecreaseTripSeats
{
    public class DecreaseTripSeatsCommandHandler : IRequestHandler<DecreaseTripSeatsCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DecreaseTripSeatsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<string>> Handle(DecreaseTripSeatsCommand request, CancellationToken cancellationToken)
        {
            var dto = request.SeatsDto;

            var trip = await _unitOfWork.TripRepository.GetByIdAsync(dto.TripId);

            if (trip == null || trip.IsDeleted || trip.TripStatus != TripStatus.Scheduled)
                return Response<string>.Fail("الرحلة غير صالحة.");

            if (trip.AvailableSeats < dto.NumberOfSeats)
                return Response<string>.Fail("لا توجد مقاعد كافية متاحة.");

            trip.AvailableSeats -= dto.NumberOfSeats;
            trip.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.TripRepository.Update(trip);
            await _unitOfWork.CompleteAsync();

            return Response<string>.Success("تم تحديث عدد المقاعد.");
        }
    }

}
