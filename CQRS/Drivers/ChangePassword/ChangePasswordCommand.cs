using MediatR;
using Microsoft.AspNetCore.Identity;
using RakbnyMa_aak.DTOs.DriverDTOs.UpdateProfileDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.CQRS.Drivers.ChangePassword
{
    public class ChangePasswordCommand : IRequest<Response<string>>
    {
        public string UserId { get; set; }
        public ChangePasswordDto Dto { get; set; }

        public ChangePasswordCommand(string userId, ChangePasswordDto dto)
        {
            UserId = userId;
            Dto = dto;
        }
    }
}
