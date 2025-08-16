using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Governorates.DeleteGovernorate
{
    public record DeleteGovernorateCommand(int Id) : IRequest<Response<string>>;
}
