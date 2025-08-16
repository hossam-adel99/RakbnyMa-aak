using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Admin.RejectDriver
{
    public record RejectDriverCommand(string DriverId, string? Reason) : IRequest<Response<bool>>;
}