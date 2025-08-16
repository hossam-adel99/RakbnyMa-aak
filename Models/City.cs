using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RakbnyMa_aak.Models
{
    public class City : BaseEntity
    {
        [Required(ErrorMessage = "اسم المدينة مطلوب.")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز اسم المدينة 100 حرف.")]
        [Display(Name = "اسم المدينة")]
        public string Name { get; set; }

        [Required(ErrorMessage = "المحافظة مطلوبة.")]
        [Display(Name = "المحافظة")]
        public int GovernorateId { get; set; }

        [ForeignKey("GovernorateId")]
        public virtual Governorate Governorate { get; set; }
    }
}
