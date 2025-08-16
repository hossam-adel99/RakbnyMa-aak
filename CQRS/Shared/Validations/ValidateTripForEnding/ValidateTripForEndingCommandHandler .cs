using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripForEnding
{
    public class ValidateTripForEndingCommandHandler : IRequestHandler<ValidateTripForEndingCommand, Response<Trip>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidateTripForEndingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<Trip>> Handle(ValidateTripForEndingCommand request, CancellationToken cancellationToken)
        {
            var tripProjection = await _unitOfWork.TripRepository
                .GetAllQueryable()
                .Where(t => t.Id == request.TripId)
                .Select(t => new Trip
                {
                    Id = t.Id,
                    DriverId = t.DriverId,
                    IsDeleted = t.IsDeleted,
                    TripStatus = t.TripStatus
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (tripProjection == null || tripProjection.IsDeleted || tripProjection.DriverId != request.DriverId)
                return Response<Trip>.Fail("غير مصرح أو لم يتم العثور على الرحلة.");

            if (tripProjection.TripStatus != TripStatus.Ongoing)
                return Response<Trip>.Fail("الرحلة لم تبدأ بعد.");

            return Response<Trip>.Success(tripProjection, "رحلة صالحة");
        }
    }
}
