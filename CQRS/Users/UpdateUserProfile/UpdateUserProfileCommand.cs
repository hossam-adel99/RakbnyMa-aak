using MediatR;
using RakbnyMa_aak.DTOs.UserDTOs.UpdateProfileDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Users.UpdateUserProfile
{
    public class UpdateUserProfileCommand : IRequest<Response<string>>
    {
        public UpdateUserProfileDto Dto { get; }
        public string UserId { get; set; }
        public UpdateUserProfileCommand(UpdateUserProfileDto dto,string userId)
        {
            Dto = dto;
            UserId = userId;
        }
    }
}
