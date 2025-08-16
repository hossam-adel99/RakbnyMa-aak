using MediatR;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Cities.UpdateCity
{
    public record UpdateCityCommand(CityDto Dto) : IRequest<Response<CityResponseDTO>>;
}
