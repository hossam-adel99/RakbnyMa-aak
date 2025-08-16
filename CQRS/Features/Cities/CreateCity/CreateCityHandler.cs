using AutoMapper;
using MediatR;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Cities.CreateCity
{
    public class CreateCityHandler : IRequestHandler<CreateCityCommand, Response<CityResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCityHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<CityResponseDTO>> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            var isExist = await _unitOfWork.CityRepository
                .AnyAsync(c => c.Name == request.Dto.Name && c.GovernorateId == request.Dto.GovernorateId);

            if (isExist)
            {
                return Response<CityResponseDTO>.Fail("هذه المدينة موجودة بالفعل في المحافظة المحددة", null);
            }

            var entity = _mapper.Map<City>(request.Dto);
            await _unitOfWork.CityRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            var savedCity = await _unitOfWork.CityRepository
                .GetCityWithGovernorateNameByIdAsync(entity.Id);

            var responseDto = _mapper.Map<CityResponseDTO>(savedCity);

            return Response<CityResponseDTO>.Success(responseDto, "تم إنشاء المدينة بنجاح");
        }

    }
}
