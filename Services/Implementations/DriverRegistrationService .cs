using Microsoft.AspNetCore.Identity;
using RakbnyMa_aak.DTOs.Auth.RequestDTOs;
using RakbnyMa_aak.DTOs.Auth.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Interfaces;
using RakbnyMa_aak.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Services.Implementations
{
    public class DriverRegistrationService : IDriverRegistrationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICloudinaryService _cloudinary;
        private readonly IDriverVerificationService _verificationService;
        private readonly IDriverRepository _repo;
        private readonly INotificationService _notificationService;

        public DriverRegistrationService(
              UserManager<ApplicationUser> userManager,
              ICloudinaryService cloudinary,
              IDriverVerificationService verificationService,
              IDriverRepository repo,
              INotificationService notificationService)
        {
            _userManager = userManager;
            _cloudinary = cloudinary;
            _verificationService = verificationService;
            _repo = repo;
            _notificationService = notificationService;
        }

        public async Task<Response<RegisterResponseDto>> RegisterDriverAsync(RegisterDriverRequestDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null ||
                await _userManager.FindByNameAsync(dto.FullName) != null)
            {
                return Response<RegisterResponseDto>.Fail("البريد الإلكتروني أو اسم المستخدم موجود بالفعل.");
            }

            // رفع الصور
            var nationalIdImgUrl = await _cloudinary.UploadImageAsync(dto.NationalIdImage, "drivers/nationalId");
            var licenseImgUrl = await _cloudinary.UploadImageAsync(dto.DriverLicenseImage, "drivers/license");
            var carLicenseImgUrl = await _cloudinary.UploadImageAsync(dto.CarLicenseImage, "drivers/carlicense");
            var selfieImgUrl = await _cloudinary.UploadImageAsync(dto.SelfieImage, "drivers/selfie");

            string profileImageUrl;
            if (dto.Picture != null && dto.Picture.Length > 0)
            {
                profileImageUrl = await _cloudinary.UploadImageAsync(dto.Picture, "users/profile");
            }
            else
            {
                profileImageUrl = "https://res.cloudinary.com/dbrz7pbsa/image/upload/v1751624539/default-profile_zo7m6z.png";
            }

            // إنشاء مستخدم جديد
            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Picture = profileImageUrl,
                UserType = UserType.Driver,
                Gender = dto.Gender
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return Response<RegisterResponseDto>.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

            // إنشاء كيان السائق
            var driver = new Driver
            {
                UserId = user.Id,
                NationalId = dto.NationalId,
                CarType = dto.CarType,
                CarColor = dto.CarColor,
                CarCapacity = dto.CarCapacity,
                CarModel = dto.CarModel,
                CarPlateNumber = dto.CarPlateNumber,
                DriverNationalIdImagePath = nationalIdImgUrl,
                DriverLicenseImagePath = licenseImgUrl,
                CarLicenseImagePath = carLicenseImgUrl,
                SelfieImagePath = selfieImgUrl,
            };

            await _repo.AddAsync(driver);
            await _repo.CompleteAsync();

            // إشعار المشرفين بطلب تسجيل السائق
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            foreach (var admin in admins)
            {
                await _notificationService.SendNotificationAsync(
                    recipientUserId: admin.Id,
                    sender: user,
                    message: $"السائق {user.FullName} قدم طلب تسجيل.",
                    type: NotificationType.DriverRegistration,
                    relatedEntityId: user.Id
                );
            }

            return Response<RegisterResponseDto>.Success(
                new RegisterResponseDto { Id = user.Id },
                "تم تسجيل السائق بنجاح ويحتاج إلى الموافقة"
            );
        }
    }
}
