using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;
using static RakbnyMa_aak.Utilities.Enums;
using TransactionStatus = RakbnyMa_aak.Utilities.Enums.TransactionStatus;

namespace RakbnyMa_aak.Models
{
    public class WalletTransaction : BaseEntity
    {
        [Required]
        [Display(Name = "المحفظة")]
        public string WalletUserId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "يجب أن يكون المبلغ أكبر من 0.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [EnumDataType(typeof(TransactionType))]
        public TransactionType TransactionType { get; set; } // Credit or Debit

        [Required]
        [EnumDataType(typeof(TransactionStatus))]
        public TransactionStatus Status { get; set; } = TransactionStatus.Completed;

        [MaxLength(500)]
        public string Description { get; set; }

        // Navigation property
        [ForeignKey("WalletUserId")]
        public virtual Wallet Wallet { get; set; }
    }
}
