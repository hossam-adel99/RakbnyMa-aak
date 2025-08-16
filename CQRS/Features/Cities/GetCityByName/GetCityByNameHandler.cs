using AutoMapper;
using MediatR;
using RakbnyMa_aak.UOW;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.CQRS.Features.Cities.GetCityByName;

public class GetCityByNameHandler : IRequestHandler<GetCityByNameQuery, Response<CityResponseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCityByNameHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<CityResponseDTO>> Handle(GetCityByNameQuery request, CancellationToken cancellationToken)
    {
        var normalizedName = request.Name.Trim().ToLower();

        var cityDto = await _unitOfWork.CityRepository.GetProjectedSingleAsync<CityResponseDTO>(
            c => c.Name.ToLower() == normalizedName,
            _mapper,
            cancellationToken); 

        if (cityDto == null)
            return Response<CityResponseDTO>.Fail("لم يتم العثور على المدينة.");

        return Response<CityResponseDTO>.Success(cityDto);
    }
}
