using RakbnyMa_aak.Models;
using System.Threading.Tasks;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessCardPayment(decimal amount, string cardToken);
        Task<PaymentResult> ProcessVodafoneCashPayment(decimal amount, string phoneNumber);
    }

    public record PaymentResult(
        PaymentStatus Status,
        string? TransactionId = null,
        string? FailureReason = null,
        string? Message = null)
    {
        public bool Success => Status == PaymentStatus.Completed;
    }
}