using MediatR;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Cities.GetCityById
{
    public record GetCityByIdQuery(int Id) : IRequest<Response<CityResponseDTO>>;
}
