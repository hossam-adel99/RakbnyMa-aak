using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Features.Cities
{
    public class CityDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المدينة مطلوب.")]
        [StringLength(50, ErrorMessage = "يجب ألا يزيد اسم المدينة عن 50 حرفًا.")]
        [Display(Name = "اسم المدينة")]
        public string Name { get; set; }

        [Required(ErrorMessage = "معرّف المحافظة مطلوب.")]
        [Display(Name = "معرّف المحافظة")]
        public int GovernorateId { get; set; }

        [Display(Name = "اسم المحافظة")]
        public string? GovernorateName { get; internal set; }
    }
}
