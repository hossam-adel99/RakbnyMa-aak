using MediatR;
using RakbnyMa_aak.DTOs.Auth.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Services.Interfaces;
namespace RakbnyMa_aak.CQRS.Features.Auth.Commands.RegisterDriver
{
    public class RegisterDriverCommandHandler : IRequestHandler<RegisterDriverCommand, Response<RegisterResponseDto>>
    {
        private readonly IDriverRegistrationService _registrationService;

        public RegisterDriverCommandHandler(IDriverRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        public async Task<Response<RegisterResponseDto>> Handle(RegisterDriverCommand request, CancellationToken cancellationToken)
        {
            return await _registrationService.RegisterDriverAsync(request.DriverDto);

        }

    }
}
