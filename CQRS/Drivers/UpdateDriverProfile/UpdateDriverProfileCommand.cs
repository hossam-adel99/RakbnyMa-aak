using MediatR;
using RakbnyMa_aak.DTOs.DriverDTOs.UpdateProfileDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Drivers.UpdateDriverProfile
{
    public class UpdateDriverProfileCommand : IRequest<Response<string>>
    {
        public string DriverUserId { get; set; }
        public UpdateDriverProfileDto Dto { get; set; }

        public UpdateDriverProfileCommand(string driverUserId, UpdateDriverProfileDto dto)
        {
            DriverUserId = driverUserId;
            Dto = dto;
        }
    }

}
