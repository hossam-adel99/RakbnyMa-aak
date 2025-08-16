using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Services.Interfaces;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Drivers.UpdateDriverCarInfo
{
    public class UpdateDriverCarInfoHandler : IRequestHandler<UpdateDriverCarInfoCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinary;

        public UpdateDriverCarInfoHandler(IUnitOfWork unitOfWork, ICloudinaryService cloudinary)
        {
            _unitOfWork = unitOfWork;
            _cloudinary = cloudinary;
        }

        public async Task<Response<string>> Handle(UpdateDriverCarInfoCommand request, CancellationToken cancellationToken)
        {
            var driver = await _unitOfWork.DriverRepository.GetByUserIdAsync(request.DriverUserId);
            if (driver == null)
                return Response<string>.Fail("لم يتم العثور على السائق.", statusCode: 404);

            driver.CarModel = request.Dto.CarModel;
            driver.CarColor = request.Dto.CarColor;
            driver.CarType = request.Dto.CarType;
            driver.CarCapacity = request.Dto.CarCapacity;
            driver.CarPlateNumber = request.Dto.CarPlateNumber;

            // رفع صورة رخصة السيارة
            if (request.Dto.CarLicenseImage != null)
            {
                var url = await _cloudinary.UploadImageAsync(request.Dto.CarLicenseImage, "drivers/carlicense");
                driver.CarLicenseImagePath = url;
            }

            // رفع صورة رخصة القيادة
            if (request.Dto.DriverLicenseImage != null)
            {
                var url = await _cloudinary.UploadImageAsync(request.Dto.DriverLicenseImage, "drivers/license");
                driver.DriverLicenseImagePath = url;
            }

            driver.IsApproved = false;

            _unitOfWork.DriverRepository.Update(driver);
            await _unitOfWork.CompleteAsync();

            return Response<string>.Success("تم تحديث معلومات السيارة بنجاح. جارٍ انتظار موافقة المسؤول.");
        }
    }
}
