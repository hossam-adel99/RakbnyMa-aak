using MediatR;
using RakbnyMa_aak.CQRS.Features.Governorates;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Governorates.CreateGovernorate
{
    public record CreateGovernorateCommand(GovernorateDto Dto) : IRequest<Response<GovernorateResponseDTO>>;
}
