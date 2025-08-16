using System.ComponentModel.DataAnnotations;

namespace RakbnyMa_aak.CQRS.Commands.Validations.CheckUserAlreadyBooked
{
    public class CheckUserAlreadyBookedDto
    {
        [Required(ErrorMessage = "معرّف الرحلة مطلوب.")]
        [Display(Name = "معرّف الرحلة")]
        public int TripId { get; set; }

        [Required(ErrorMessage = "معرّف المستخدم مطلوب.")]
        [Display(Name = "معرّف المستخدم")]
        public string UserId { get; set; }
    }
}
