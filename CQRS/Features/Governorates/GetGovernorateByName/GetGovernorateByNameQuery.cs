using MediatR;
using RakbnyMa_aak.CQRS.Features.Governorates;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Governorates.GetGovernorateByName
{
    public record GetGovernorateByNameQuery(string Name) : IRequest<Response<GovernorateDto>>;
}
