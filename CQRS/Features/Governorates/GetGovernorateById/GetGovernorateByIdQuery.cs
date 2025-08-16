using MediatR;
using RakbnyMa_aak.CQRS.Features.Governorates;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Governorates.GetGovernorateById
{
    public record GetGovernorateByIdQuery(int Id) : IRequest<Response<GovernorateDto>>;
}
