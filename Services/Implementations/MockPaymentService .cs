using RakbnyMa_aak.Models;
using RakbnyMa_aak.Services.Interfaces;
using System;
using System.Threading.Tasks;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Services.Implementations
{
    public class MockPaymentService : IPaymentService
    {
        private const decimal MAX_CARD_AMOUNT = 10000m;
        private const decimal MAX_VODAFONE_AMOUNT = 5000m;

        public async Task<PaymentResult> ProcessCardPayment(decimal amount, string cardToken)
        {
            // Simulate API call delay
            await Task.Delay(500);

            if (string.IsNullOrWhiteSpace(cardToken))
                return new PaymentResult(
                    PaymentStatus.Failed,
                    FailureReason: "رمز البطاقة غير صالح",
                    Message: "رمز البطاقة مطلوب");

            if (amount <= 0)
                return new PaymentResult(
                    PaymentStatus.Failed,
                    FailureReason: "المبلغ غير صالح",
                    Message: "يجب أن يكون المبلغ موجباً");

            if (amount > MAX_CARD_AMOUNT)
                return new PaymentResult(
                    PaymentStatus.Failed,
                    FailureReason: "تم تجاوز حد المبلغ",
                    Message: $"الحد الأقصى للدفع بالبطاقة هو {MAX_CARD_AMOUNT} جنيه");

            // Simulate random failures (10% chance)
            if (new Random().Next(0, 10) == 0)
                return new PaymentResult(
                    PaymentStatus.Failed,
                    FailureReason: "انتهت مهلة بوابة الدفع",
                    Message: "انتهت مهلة معالجة الدفع");

            return new PaymentResult(
                PaymentStatus.Completed,
                TransactionId: $"CARD_{DateTime.UtcNow.Ticks}_{Guid.NewGuid().ToString().Substring(0, 8)}",
                Message: "تمت معالجة الدفع بنجاح");
        }

        public async Task<PaymentResult> ProcessVodafoneCashPayment(decimal amount, string phoneNumber)
        {
            // Simulate API call delay
            await Task.Delay(500);

            if (string.IsNullOrWhiteSpace(phoneNumber) || !phoneNumber.StartsWith("01"))
                return new PaymentResult(
                    PaymentStatus.Failed,
                    FailureReason: "رقم الهاتف غير صالح",
                    Message: "رقم هاتف مصري صالح مطلوب");

            if (amount <= 0)
                return new PaymentResult(
                    PaymentStatus.Failed,
                    FailureReason: "المبلغ غير صالح",
                    Message: "يجب أن يكون المبلغ موجباً");

            if (amount > MAX_VODAFONE_AMOUNT)
                return new PaymentResult(
                    PaymentStatus.Failed,
                    FailureReason: "تم تجاوز حد المبلغ",
                    Message: $"الحد الأقصى للدفع عبر فودافون كاش هو {MAX_VODAFONE_AMOUNT} جنيه");

            // Simulate random failures (15% chance)
            if (new Random().Next(0, 7) == 0)
                return new PaymentResult(
                    PaymentStatus.Failed,
                    FailureReason: "رصيد المحفظة غير كافٍ",
                    Message: "ليس لدى المستخدم رصيد كافٍ في فودافون كاش");

            return new PaymentResult(
                PaymentStatus.Completed,
                TransactionId: $"VODA_{DateTime.UtcNow.Ticks}",
                Message: "تم الدفع عبر فودافون كاش بنجاح");
        }
    }
}
