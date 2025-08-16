using MediatR;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Cities.GetCitiesByGovernorateId
{

    public record GetCitiesByGovernorateIdQuery(int GovernorateId) : IRequest<Response<List<CityResponseDTO>>>;
}
