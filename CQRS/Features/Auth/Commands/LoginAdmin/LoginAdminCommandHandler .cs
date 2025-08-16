using MediatR;
using RakbnyMa_aak.CQRS.Features.Auth.Commands.LoginDriver;
using RakbnyMa_aak.DTOs.Auth.Responses;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Services.Interfaces;
using RakbnyMa_aak.Utilities;

namespace RakbnyMa_aak.CQRS.Features.Auth.Commands.LoginAdmin
{
    public class LoginAdminCommandHandler : IRequestHandler<LoginDriverCommand, Response<AuthResponseDto>>
    {
        private readonly IAuthService _authService;

        public LoginAdminCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Response<AuthResponseDto>> Handle(LoginDriverCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LoginByUserTypeAsync(request.Dto, Enums.UserType.Driver);
        }
    }
}
