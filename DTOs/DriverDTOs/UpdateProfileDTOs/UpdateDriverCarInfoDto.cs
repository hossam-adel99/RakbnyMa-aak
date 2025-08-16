using static RakbnyMa_aak.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.DTOs.DriverDTOs.UpdateProfileDTOs
{
    public class UpdateDriverCarInfoDto
    {
        [Display(Name = "نوع السيارة")]
        [Required(ErrorMessage = "نوع السيارة مطلوب.")]
        public CarType CarType { get; set; }

        [Required(ErrorMessage = "موديل السيارة مطلوب. مثال (Kia Cerato 2023)")]
        [StringLength(50, ErrorMessage = "يجب ألا يزيد موديل السيارة عن 50 حرفًا. مثال (Kia Cerato 2023)")]
        [Display(Name = "موديل السيارة")]
        public string CarModel { get; set; }

        [Required(ErrorMessage = "لون السيارة مطلوب.")]
        [Display(Name = "لون السيارة")]
        public CarColor CarColor { get; set; }

        [Display(Name = "سعة السيارة")]
        [Required(ErrorMessage = "سعة السيارة مطلوبة.")]
        [Range(2, 150, ErrorMessage = "يجب أن تكون سعة السيارة بين 2 و 150.")]
        public int CarCapacity { get; set; }

        [Required(ErrorMessage = "رقم لوحة السيارة مطلوب.")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "يجب أن يكون رقم لوحة السيارة بين 2 و 10 أحرف.")]
        public string CarPlateNumber { get; set; }

        [Display(Name = "صورة رخصة القيادة")]
        [Required(ErrorMessage = "صورة رخصة القيادة مطلوبة.")]
        public IFormFile DriverLicenseImage { get; set; }

        [Display(Name = "صورة رخصة السيارة")]
        [Required(ErrorMessage = "صورة رخصة السيارة مطلوبة.")]
        public IFormFile CarLicenseImage { get; set; }
    }
}
