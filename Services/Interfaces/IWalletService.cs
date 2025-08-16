using RakbnyMa_aak.Models;
using System.Threading.Tasks;

namespace RakbnyMa_aak.Services.Interfaces
{
    public interface IWalletService
    {
        Task<decimal> GetBalanceAsync(string userId);
        Task<WalletTransaction> AddFundsAsync(string userId, decimal amount, string description);
        Task<WalletTransaction> DeductFundsAsync(string userId, decimal amount, string description);
    }
}