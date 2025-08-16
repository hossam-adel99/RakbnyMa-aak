using MediatR;
using RakbnyMa_aak.DTOs.Auth.Responses;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Services.Interfaces;
using static RakbnyMa_aak.Utilities.Enums;
namespace RakbnyMa_aak.CQRS.Features.Admin.Login
{
    public class LoginAdminCommandHandler : IRequestHandler<LoginAdminCommand, Response<AuthResponseDto>>
    {
        private readonly IAuthService _authService;

        public LoginAdminCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Response<AuthResponseDto>> Handle(LoginAdminCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LoginByUserTypeAsync(request.LoginDto, UserType.Admin);
        }
    }

}
