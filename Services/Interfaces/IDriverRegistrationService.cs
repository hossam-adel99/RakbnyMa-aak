using RakbnyMa_aak.DTOs.Auth.RequestDTOs;
using RakbnyMa_aak.DTOs.Auth.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.Services.Interfaces
{
    public interface IDriverRegistrationService
    {
        public Task<Response<RegisterResponseDto>> RegisterDriverAsync(RegisterDriverRequestDto dto);

    }
}
