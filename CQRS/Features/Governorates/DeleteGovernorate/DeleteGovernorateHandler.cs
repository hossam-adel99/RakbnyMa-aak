using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Governorates.DeleteGovernorate
{
    public class DeleteGovernorateHandler : IRequestHandler<DeleteGovernorateCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteGovernorateHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<Response<string>> Handle(DeleteGovernorateCommand request, CancellationToken cancellationToken)
        {
            var governorate = await _unitOfWork.GovernorateRepository.GetByIdAsync(request.Id);

            if (governorate == null || governorate.IsDeleted)
                return Response<string>.Fail("لم يتم العثور على المحافظة.");

            // حذف ناعم للمحافظة
            governorate.IsDeleted = true;
            governorate.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.GovernorateRepository.Update(governorate);

            // جلب المدن المرتبطة
            var relatedCities = await _unitOfWork.CityRepository.GetAllAsync(c => c.GovernorateId == request.Id && !c.IsDeleted);

            foreach (var city in relatedCities)
            {
                city.IsDeleted = true;
                city.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.CityRepository.Update(city);
            }

            await _unitOfWork.CompleteAsync();
            return Response<string>.Success("تم حذف المحافظة والمدن التابعة لها حذفًا ناعمًا بنجاح.");
        }
    }
}
