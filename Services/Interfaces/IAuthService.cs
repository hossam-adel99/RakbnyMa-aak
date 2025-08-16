using RakbnyMa_aak.DTOs.Auth.Requests;
using RakbnyMa_aak.DTOs.Auth.Responses;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Utilities;

namespace RakbnyMa_aak.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Response<AuthResponseDto>> LoginByUserTypeAsync(LoginRequestDto dto, Enums.UserType user);

    }
}
