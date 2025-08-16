using MediatR;
using RakbnyMa_aak.CQRS.Features.Governorates;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Governorates.UpdateGovernorate
{
    public record UpdateGovernorateCommand(GovernorateDto Dto) : IRequest<Response<GovernorateDto>>;
}
