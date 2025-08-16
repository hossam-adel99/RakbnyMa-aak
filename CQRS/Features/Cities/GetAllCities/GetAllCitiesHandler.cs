using AutoMapper;
using MediatR;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Cities.GetAllCities
{
    public class GetAllCitiesHandler : IRequestHandler<GetAllCitiesQuery, Response<PaginatedResult<CityDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCitiesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedResult<CityDto>>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            var dto = request.RequestDto;
            var paginatedCities = await _unitOfWork.CityRepository.GetPaginatedAsync(
                pageNumber: dto.Page,
                pageSize: dto.PageSize
            );

            var dtoItems = _mapper.Map<List<CityDto>>(paginatedCities.Items);

            var result = new PaginatedResult<CityDto>(
                dtoItems,
                paginatedCities.TotalCount,
                dto.Page,
                dto.PageSize
            );

            return Response<PaginatedResult<CityDto>>.Success(result);
        }
    }
}
