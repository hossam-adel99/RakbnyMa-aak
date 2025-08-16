using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.TripDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripBusinessRules
{
    public class ValidateTripBusinessRulesCommandHandler : IRequestHandler<ValidateTripBusinessRulesCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidateTripBusinessRulesCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(ValidateTripBusinessRulesCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Trip;

            if (dto.AvailableSeats <= 0)
                return Response<bool>.Fail("يجب أن يكون عدد المقاعد المتاحة أكبر من صفر.");

            if (dto.TripDate < DateTime.UtcNow.Date)
                return Response<bool>.Fail("يجب أن يكون تاريخ الرحلة في المستقبل.");

            var carSeats = await _unitOfWork.DriverRepository
                    .GetAllQueryable()  
                    .Where(d => d.UserId == dto.DriverId)
                    .Select(d => d.CarCapacity)
                    .FirstOrDefaultAsync();

            if (carSeats == 0)
                return Response<bool>.Fail("لم يتم العثور على السائق.");

            if (dto.AvailableSeats > carSeats)
                return Response<bool>.Fail($"لا يمكن أن يتجاوز عدد المقاعد المتاحة عدد مقاعد السيارة ({carSeats}).");

            return Response<bool>.Success(true);
        }
    }
}
