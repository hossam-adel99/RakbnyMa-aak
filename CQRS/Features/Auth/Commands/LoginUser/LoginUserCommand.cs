using MediatR;
using RakbnyMa_aak.DTOs.Auth.Requests;
using RakbnyMa_aak.DTOs.Auth.Responses;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Auth.Commands.LoginUser
{
    public record LoginUserCommand(LoginRequestDto Dto) : IRequest<Response<AuthResponseDto>>;
}
