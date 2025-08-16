namespace RakbnyMa_aak.Services.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file, string folder);

    }
}
