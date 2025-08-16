using MediatR;
using RakbnyMa_aak.DTOs.Auth;
using RakbnyMa_aak.DTOs.Auth.Requests;
using RakbnyMa_aak.DTOs.Auth.Responses;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Admin.Login
{
    public record LoginAdminCommand(LoginRequestDto LoginDto) : IRequest<Response<AuthResponseDto>>;

}
