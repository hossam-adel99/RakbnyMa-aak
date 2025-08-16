using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Cities.GetCityById
{
    public class GetCityByIdHandler : IRequestHandler<GetCityByIdQuery, Response<CityResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCityByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<CityResponseDTO>> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            var cityDto = await _unitOfWork.CityRepository.GetProjectedSingleAsync<CityResponseDTO>(
                c => c.Id == request.Id,
                _mapper,
                cancellationToken);

            if (cityDto == null)
                return Response<CityResponseDTO>.Fail("لم يتم العثور على المدينة.");

            return Response<CityResponseDTO>.Success(cityDto);
        }
    }
}
