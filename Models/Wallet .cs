using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RakbnyMa_aak.Models
{
    public class Wallet : BaseEntity
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "يجب أن يكون الرصيد إيجابيًا.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "الرصيد الحالي")]
        public decimal Balance { get; set; } = 0;

        [Display(Name = "آخر تحديث")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ApplicationUser User { get; set; }

        // Transactions history
        public virtual ICollection<WalletTransaction> Transactions { get; set; } = new HashSet<WalletTransaction>();
    }
}
