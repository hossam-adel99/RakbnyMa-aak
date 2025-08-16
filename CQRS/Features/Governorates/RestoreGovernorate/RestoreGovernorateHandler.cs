using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Governorates.RestoreGovernorate
{
    public class RestoreGovernorateHandler : IRequestHandler<RestoreGovernorateCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RestoreGovernorateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<string>> Handle(RestoreGovernorateCommand request, CancellationToken cancellationToken)
        {

            var governorate = await _unitOfWork.GovernorateRepository
                .GetAllQueryable()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (governorate == null)
                return Response<string>.Fail("لم يتم العثور على المحافظة.");

            if (!governorate.IsDeleted)
                return Response<string>.Fail("المحافظة نشطة بالفعل.");


            governorate.IsDeleted = false;
            governorate.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.GovernorateRepository.Update(governorate);


            var cities = await _unitOfWork.CityRepository
                .GetAllQueryable()
                .IgnoreQueryFilters()
                .Where(c => c.GovernorateId == governorate.Id && c.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var city in cities)
            {
                city.IsDeleted = false;
                city.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.CityRepository.Update(city);
            }

            await _unitOfWork.CompleteAsync();

            return Response<string>.Success("تمت استعادة المحافظة ومدنها بنجاح.");
        }
    }
}
