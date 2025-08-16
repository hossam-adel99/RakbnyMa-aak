using MediatR;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Cities.GetCityByName
{
    public record GetCityByNameQuery(string Name) : IRequest<Response<CityResponseDTO>>;
}
