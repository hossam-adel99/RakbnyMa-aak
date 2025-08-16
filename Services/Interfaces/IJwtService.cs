using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.Services.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateToken(ApplicationUser user);

    }
}
