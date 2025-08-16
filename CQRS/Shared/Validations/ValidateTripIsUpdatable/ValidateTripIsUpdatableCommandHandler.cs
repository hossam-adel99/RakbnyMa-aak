using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripIsUpdatable
{
    public class ValidateTripIsUpdatableCommandHandler : IRequestHandler<ValidateTripIsUpdatableCommand, Response<Trip>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidateTripIsUpdatableCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<Trip>> Handle(ValidateTripIsUpdatableCommand request, CancellationToken cancellationToken)
        {
            var trip = await _unitOfWork.TripRepository.GetByIdAsync(request.TripId);

            if (trip == null)
                return Response<Trip>.Fail("لم يتم العثور على الرحلة.");

            if (trip.IsDeleted)
                return Response<Trip>.Fail("تم حذف الرحلة.");

            if (trip.TripStatus != TripStatus.Scheduled)
                return Response<Trip>.Fail("يمكن فقط تعديل الرحلات المجدولة.");

            ///لو فه بوكينج كونفيرميد ممنوع التعديل

            return Response<Trip>.Success(trip);
        }
    }
}
