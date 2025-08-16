using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateCityInGovernorate
{
    public class ValidateCityInGovernorateHandler : IRequestHandler<ValidateCityInGovernorateCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidateCityInGovernorateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(ValidateCityInGovernorateCommand request, CancellationToken cancellationToken)
        {
            var city = await _unitOfWork.CityRepository.GetByIdAsync(request.CityId);

            if (city == null)
                return Response<bool>.Fail("لم يتم العثور على المدينة.");

            if (city.GovernorateId != request.GovernorateId)
                return Response<bool>.Fail("المدينة لا تنتمي إلى المحافظة المحددة.");

            return Response<bool>.Success(true, "المدينة تنتمي إلى المحافظة بنجاح.");
        }
    }
}
