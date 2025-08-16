using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.Models
{
    public class Governorate : BaseEntity
    {
        [Required(ErrorMessage = "اسم المحافظة مطلوب.")]
        [MaxLength(100, ErrorMessage = "يجب ألا يزيد اسم المحافظة عن 100 حرف.")]
        [Display(Name = "اسم المحافظة")]
        public string Name { get; set; }

        [Display(Name = "المدن")]
        public virtual ICollection<City> Cities { get; set; }

        public Governorate()
        {
            Cities = new HashSet<City>();
        }
    }
}
