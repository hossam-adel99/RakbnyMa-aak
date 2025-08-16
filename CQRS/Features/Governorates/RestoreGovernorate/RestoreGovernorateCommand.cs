using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Governorates.RestoreGovernorate
{
    public record RestoreGovernorateCommand(int Id) : IRequest<Response<string>>;
}
