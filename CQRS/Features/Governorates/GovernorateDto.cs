using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Features.Governorates
{
    public class GovernorateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المحافظة مطلوب.")]
        [MaxLength(50, ErrorMessage = "يجب ألا يزيد اسم المحافظة عن 50 حرفًا.")]
        [Display(Name = "اسم المحافظة")]
        public string Name { get; set; }
    }
}
