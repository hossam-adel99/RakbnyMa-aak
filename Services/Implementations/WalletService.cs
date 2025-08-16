using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Services.Interfaces;
using System;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.Services.Implementations
{
    public class WalletService : IWalletService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<WalletService> _logger;

        public WalletService(AppDbContext context, ILogger<WalletService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<decimal> GetBalanceAsync(string userId)
        {
            var wallet = await _context.Wallets
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.UserId == userId);

            return wallet?.Balance ?? 0;
        }

        public async Task<WalletTransaction> AddFundsAsync(string userId, decimal amount, string description)
        {
            if (amount <= 0)
                throw new ArgumentException("يجب أن يكون المبلغ أكبر من الصفر", nameof(amount));

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var wallet = await _context.Wallets
                    .Include(w => w.Transactions)
                    .FirstOrDefaultAsync(w => w.UserId == userId);

                if (wallet == null)
                {
                    wallet = new Wallet
                    {
                        UserId = userId,
                        Balance = 0
                    };
                    _context.Wallets.Add(wallet);
                }

                wallet.Balance += amount;
                wallet.LastUpdated = DateTime.UtcNow;

                var transactionRecord = new WalletTransaction
                {
                    WalletUserId = userId,
                    Amount = amount,
                    TransactionType = TransactionType.Credit,
                    Description = description,
                    Status = TransactionStatus.Completed
                };

                wallet.Transactions.Add(transactionRecord);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return transactionRecord;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "فشل في إضافة الأموال إلى المحفظة للمستخدم {UserId}", userId);
                throw;
            }
        }

        public async Task<WalletTransaction> DeductFundsAsync(string userId, decimal amount, string description)
        {
            if (amount <= 0)
                throw new ArgumentException("يجب أن يكون المبلغ أكبر من الصفر", nameof(amount));

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var wallet = await _context.Wallets
                    .Include(w => w.Transactions)
                    .FirstOrDefaultAsync(w => w.UserId == userId);

                if (wallet == null || wallet.Balance < amount)
                    throw new InvalidOperationException("الرصيد غير كافٍ");

                wallet.Balance -= amount;
                wallet.LastUpdated = DateTime.UtcNow;

                var transactionRecord = new WalletTransaction
                {
                    WalletUserId = userId,
                    Amount = amount,
                    TransactionType = TransactionType.Debit,
                    Description = description,
                    Status = TransactionStatus.Completed
                };

                wallet.Transactions.Add(transactionRecord);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return transactionRecord;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "فشل في خصم الأموال من المحفظة للمستخدم {UserId}", userId);
                throw;
            }
        }
    }
}
