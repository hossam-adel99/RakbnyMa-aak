using MediatR;
using RakbnyMa_aak.DTOs.DriverDTOs.UpdateProfileDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Drivers.UpdateDriverCarInfo
{
    public class UpdateDriverCarInfoCommand : IRequest<Response<string>>
    {
        public string DriverUserId { get; set; }
        public UpdateDriverCarInfoDto Dto { get; set; }

        public UpdateDriverCarInfoCommand(string driverUserId, UpdateDriverCarInfoDto dto)
        {
            DriverUserId = driverUserId;
            Dto = dto;
        }
    }

}
