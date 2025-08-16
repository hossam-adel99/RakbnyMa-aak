using MediatR;
using RakbnyMa_aak.DTOs.Auth.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Services.Interfaces;
namespace RakbnyMa_aak.CQRS.Features.Auth.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<RegisterResponseDto>>
    {
        private readonly IUserRegistrationService _registrationService;

        public RegisterUserCommandHandler(IUserRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        public async Task<Response<RegisterResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            return await _registrationService.RegisterUserAsync(request.Dto);
        }
    }
}
