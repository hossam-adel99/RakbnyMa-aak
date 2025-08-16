using MediatR;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Cities.CreateCity
{
    public record CreateCityCommand(CityDto Dto) : IRequest<Response<CityResponseDTO>>;
}
