using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RakbnyMa_aak.DTOs.Auth.RequestDTOs;
using RakbnyMa_aak.DTOs.Auth.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Services.Interfaces;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Services.Implementations
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinary;

        public UserRegistrationService(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ICloudinaryService cloudinary)
        {
            _userManager = userManager;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        public async Task<Response<RegisterResponseDto>> RegisterUserAsync(RegisterUserRequestDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null ||
                await _userManager.FindByNameAsync(dto.FullName) != null)
            {
                return Response<RegisterResponseDto>.Fail("البريد الإلكتروني مستخدم من قبل.");
            }

            string pictureUrl = null;
            if (dto.Picture != null)
            {
                pictureUrl = await _cloudinary.UploadImageAsync(dto.Picture, "users/profile");
            }
            else
            {
                pictureUrl = "https://res.cloudinary.com/dbrz7pbsa/image/upload/v1751624539/default-profile_zo7m6z.png";
            }

            var user = _mapper.Map<ApplicationUser>(dto);
            user.Email = dto.Email;
            user.UserName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            user.UserType = UserType.User;
            user.Gender = dto.Gender;
            user.Picture = pictureUrl;

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Response<RegisterResponseDto>.Fail(errors);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return Response<RegisterResponseDto>.Fail("تم إنشاء المستخدم ولكن فشلت إضافة الدور: " + errors);
            }

            return Response<RegisterResponseDto>.Success(new RegisterResponseDto { Id = user.Id },
                "تم تسجيل المستخدم بنجاح.");
        }
    }
}
