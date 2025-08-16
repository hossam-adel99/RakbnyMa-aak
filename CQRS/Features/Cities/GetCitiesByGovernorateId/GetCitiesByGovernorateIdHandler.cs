using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Cities.GetCitiesByGovernorateId
{
    public class GetCitiesByGovernorateIdHandler : IRequestHandler<GetCitiesByGovernorateIdQuery, Response<List<CityResponseDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCitiesByGovernorateIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<List<CityResponseDTO>>> Handle(GetCitiesByGovernorateIdQuery request, CancellationToken cancellationToken)
        {

            var citiesDto = await _unitOfWork.CityRepository
                    .GetProjectedListAsync<CityResponseDTO>(
                        c => c.GovernorateId == request.GovernorateId,
                        _mapper,
                        cancellationToken
                    );

            if (!citiesDto.Any())
                return Response<List<CityResponseDTO>>.Fail("لم يتم العثور على مدن لهذه المحافظة.");

            return Response<List<CityResponseDTO>>.Success(citiesDto);

        }
    }
}
