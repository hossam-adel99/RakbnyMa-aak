namespace RakbnyMa_aak.Services.Interfaces
{
    public interface IDriverVerificationService
    {
        Task<bool> MatchFaceAsync(string selfieUrl, string nationalIdUrl);
    }
}
