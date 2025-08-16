using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.DTOs.Shared;
using RakbnyMa_aak.CQRS.Features.Governorates;


namespace RakbnyMa_aak.CQRS.Features.Governorates.GetAllGovernorates
{
    public record GetAllGovernoratesQuery(GetAllRequestDto RequestDto) : IRequest<Response<PaginatedResult<GovernorateDto>>>;

}
