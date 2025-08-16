using MediatR;
using Microsoft.AspNetCore.Identity;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Services.Interfaces;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Drivers.UpdateDriverProfile
{
    public class UpdateDriverProfileHandler : IRequestHandler<UpdateDriverProfileCommand, Response<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinary;

        public UpdateDriverProfileHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, ICloudinaryService cloudinary)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _cloudinary = cloudinary;
        }

        public async Task<Response<string>> Handle(UpdateDriverProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.DriverUserId);
            if (user == null)
                return Response<string>.Fail("المستخدم غير موجود", statusCode: 404);

            var driver = await _unitOfWork.DriverRepository.GetByUserIdAsync(user.Id);
            if (driver == null)
                return Response<string>.Fail("السائق غير موجود", statusCode: 404);

            user.FullName = request.Dto.FullName;
            user.Email = request.Dto.Email;
            user.PhoneNumber = request.Dto.PhoneNumber;

            // upload profileImageUrl
            string profileImageUrl;
            if (request.Dto.Picture != null && request.Dto.Picture.Length > 0)
            {
                profileImageUrl = await _cloudinary.UploadImageAsync(request.Dto.Picture, "users/profile");
                user.Picture = profileImageUrl;

            }
            else
            {
                profileImageUrl = "https://res.cloudinary.com/dbrz7pbsa/image/upload/v1751624539/default-profile_zo7m6z.png";
                user.Picture = profileImageUrl;
            }

            // upload SelfieImage
            if (request.Dto.SelfieImage != null)
            {
                var selfieUrl = await _cloudinary.UploadImageAsync(request.Dto.SelfieImage, "drivers/selfie");
                driver.SelfieImagePath = selfieUrl;
            }

            // upload NationalIdImage
            if (request.Dto.NationalIdImage != null)
            {
                var nationalIdUrl = await _cloudinary.UploadImageAsync(request.Dto.NationalIdImage, "drivers/nationalId");
                driver.DriverNationalIdImagePath = nationalIdUrl;
            }

            driver.IsApproved = false;

            _unitOfWork.DriverRepository.Update(driver);
            await _unitOfWork.CompleteAsync();

            return Response<string>.Success("تم تحديث الملف الشخصي بنجاح. الرجاء انتظار موافقة المسؤول.");
        }
    }
}
