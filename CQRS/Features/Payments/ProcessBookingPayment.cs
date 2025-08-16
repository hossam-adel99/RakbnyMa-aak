using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Features.Payments
{
    public static class ProcessBookingPayment
    {
        public class Command : IRequest<Result>
        {
            [Required(ErrorMessage = "رقم الحجز مطلوب.")]
            public int BookingId { get; set; }

            [Required(ErrorMessage = "طريقة الدفع مطلوبة.")]
            [EnumDataType(typeof(PaymentMethod), ErrorMessage = "طريقة الدفع غير صحيحة.")]
            public PaymentMethod PaymentMethod { get; set; }

            public string? CardToken { get; set; }
            public string? PhoneNumber { get; set; }
        }

        public class Result
        {
            public int PaymentId { get; set; }
            public PaymentStatus Status { get; set; }
            public string? TransactionId { get; set; }
            public string? Message { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly AppDbContext _context;
            private readonly IPaymentService _paymentService;
            private readonly ILogger<Handler> _logger;

            public Handler(
                AppDbContext context,
                IPaymentService paymentService,
                ILogger<Handler> logger)
            {
                _context = context;
                _paymentService = paymentService;
                _logger = logger;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var booking = await _context.Bookings
                        .Include(b => b.User)
                            .ThenInclude(u => u.Wallet)
                                .ThenInclude(w => w.Transactions)
                       .Where(b => b.Id == request.BookingId &&
                            !_context.Payments.Any(p => p.BookingId == b.Id))
                        .Select(b => new
                        {
                            Booking = b,
                            b.UserId,
                            b.TotalPrice,
                            b.User.Wallet,
                            b.User.PhoneNumber
                        })
                        .FirstOrDefaultAsync(cancellationToken);

                    if (booking == null)
                    {
                        throw new Exception("لم يتم العثور على الحجز أو تم الدفع مسبقًا");
                    }

                    var payment = new Payment
                    {
                        UserId = booking.UserId,
                        BookingId = request.BookingId,
                        Amount = booking.TotalPrice,
                        PaymentMethod = request.PaymentMethod,
                        PaymentType = PaymentType.BookingPayment,
                        PaymentStatus = PaymentStatus.Pending
                    };

                    _context.Payments.Add(payment);
                    await _context.SaveChangesAsync(cancellationToken);

                    // Use the interface's PaymentResult consistently
                    Services.Interfaces.PaymentResult paymentResult = request.PaymentMethod switch
                    {
                        PaymentMethod.Wallet => await ProcessWalletPayment(
                            booking.Wallet,
                            booking.TotalPrice,
                            $"Booking #{request.BookingId}"),

                        PaymentMethod.CreditCard => await _paymentService.ProcessCardPayment(
                            booking.TotalPrice,
                            request.CardToken),

                        PaymentMethod.VodafoneCash => await _paymentService.ProcessVodafoneCashPayment(
                            booking.TotalPrice,
                            request.PhoneNumber ?? booking.PhoneNumber),

                        _ => new Services.Interfaces.PaymentResult(
                            PaymentStatus.Completed,
                            TransactionId: "CASH_PAYMENT")
                    };

                    payment.PaymentStatus = paymentResult.Status;
                    payment.TransactionId = paymentResult.TransactionId;
                    payment.FailureReason = paymentResult.FailureReason;
                    payment.PaymentDate = paymentResult.Success ? DateTime.UtcNow : null;

                    if (paymentResult.Success)
                    {
                        booking.Booking.RequestStatus = RequestStatus.Confirmed;
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    return new Result
                    {
                        PaymentId = payment.Id,
                        Status = payment.PaymentStatus,
                        TransactionId = payment.TransactionId,
                        Message = paymentResult.Message
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "فشل الدفع للحجز {BookingId}", request.BookingId);
                    throw;
                }
            }

            private async Task<Services.Interfaces.PaymentResult> ProcessWalletPayment(
                Wallet wallet,
                decimal amount,
                string description)
            {
                if (wallet == null)
                    throw new Exception("محفظة المستخدم غير مفعلة");

                if (wallet.Balance < amount)
                {
                    return new Services.Interfaces.PaymentResult(
                        PaymentStatus.Failed,
                        FailureReason: "الرصيد غير كافٍ");
                }

                wallet.Balance -= amount;
                wallet.LastUpdated = DateTime.UtcNow;

                wallet.Transactions.Add(new WalletTransaction
                {
                    Amount = amount,
                    TransactionType = TransactionType.Debit,
                    Description = description,
                    Status = TransactionStatus.Completed
                });

                return new Services.Interfaces.PaymentResult(
                    PaymentStatus.Completed,
                    TransactionId: "WALLET_PAYMENT");
            }
        }
    }
}
