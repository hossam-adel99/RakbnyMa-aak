using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using RakbnyMa_aak.Services.Interfaces;

namespace RakbnyMa_aak.Services.Implementations
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration config)
        {
            var acc = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]);

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("الملف فارغ.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("مسموح فقط بملفات الصور (.jpg، .jpeg، .png، .gif).");

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };
            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("فشل رفع الصورة. يرجى المحاولة مرة أخرى.");
            // Validate file size
            //if (file.Length > 5 * 1024 * 1024) // 5MB
            //    throw new ArgumentException("يجب ألا يتجاوز حجم الصورة 5 ميجابايت.");

            return result.SecureUrl.ToString();
        }

    }

}
