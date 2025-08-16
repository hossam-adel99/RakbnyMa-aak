using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.Auth.Requests;
using RakbnyMa_aak.DTOs.Auth.Responses;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Services.Interfaces;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<Response<AuthResponseDto>> LoginByUserTypeAsync(LoginRequestDto dto, UserType expectedType)
        {
            var user = await _userManager.FindByEmailAsync(dto.EmailOrUsername)
                       ?? await _userManager.FindByNameAsync(dto.EmailOrUsername);

            if (user == null)
                return Response<AuthResponseDto>.Fail("المستخدم غير موجود");

            if (user.UserType != expectedType)
                return Response<AuthResponseDto>.Fail("تسجيل دخول غير مصرح به لهذا الدور");

            // Check driver approval status
            if (expectedType == UserType.Driver)
            {
                var isApproved = await _userManager.Users
                    .Where(u => u.Id == user.Id)
                    .Select(u => u.Driver.IsApproved)
                    .FirstOrDefaultAsync();

                if (!isApproved)
                    return Response<AuthResponseDto>.Fail("السائق لم يتم الموافقة عليه بعد.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return Response<AuthResponseDto>.Fail("بيانات الاعتماد غير صحيحة");

            var token = await _jwtService.GenerateToken(user);

            return Response<AuthResponseDto>.Success(new AuthResponseDto
            {
                UserId = user.Id,
                Token = token,
                FullName = user.FullName,
                Role = user.UserType.ToString()
            }, "تم تسجيل الدخول بنجاح");
        }
    }

}
