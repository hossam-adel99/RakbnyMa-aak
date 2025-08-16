using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Cities.DeleteCity
{

    public class DeleteCityHandler : IRequestHandler<DeleteCityCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCityHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<Response<string>> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {

            var city = await _unitOfWork.CityRepository.GetByIdAsync(request.Id);
            if (city == null || city.IsDeleted)
                return Response<string>.Fail("لم يتم العثور على المدينة.");

            city.IsDeleted = true;
            city.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.CityRepository.Update(city);
            await _unitOfWork.CompleteAsync();

            return Response<string>.Success("تم حذف المدينة حذفاً منطقياً بنجاح.");
        }
    }
}
