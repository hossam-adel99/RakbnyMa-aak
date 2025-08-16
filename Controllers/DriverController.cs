using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RakbnyMa_aak.CQRS.Drivers.ChangePassword;
using RakbnyMa_aak.CQRS.Drivers.UpdateDriverCarInfo;
using RakbnyMa_aak.CQRS.Drivers.UpdateDriverProfile;
using RakbnyMa_aak.CQRS.Features.Auth.Queries.GetDriverById;
using RakbnyMa_aak.CQRS.Queries.Driver.GetDriverCompletionRate;
using RakbnyMa_aak.CQRS.Queries.Driver.GetDriverMonthlyStats;
using RakbnyMa_aak.CQRS.Queries.Driver.GetDriverTotalEarnings.RakbnyMa_aak.CQRS.Queries.Driver.GetDriverTotalEarnings;
using RakbnyMa_aak.CQRS.Queries.Driver.GetDriverTripCount;
using RakbnyMa_aak.DTOs.DriverDTOs.ResponseDTOs;
using RakbnyMa_aak.DTOs.DriverDTOs.UpdateProfileDTOs;
using RakbnyMa_aak.GeneralResponse;
using System.Security.Claims;

namespace RakbnyMa_aak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DriverController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _mediator.Send(new ChangePasswordCommand(userId, dto));
                return result.IsSucceeded ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred while changing the password.",
                    Error = ex.Message
                });
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpPatch("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateDriverProfileDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _mediator.Send(new UpdateDriverProfileCommand(userId, dto));
                return result.IsSucceeded ? Ok(result) : BadRequest(result);
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.Fail($"Unexpected error: {ex.Message}"));
            }
        }



        [Authorize(Roles = "Driver")]
        [HttpPatch("update-car-info")]
        public async Task<IActionResult> UpdateCarInfo([FromForm] UpdateDriverCarInfoDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = await _mediator.Send(new UpdateDriverCarInfoCommand(userId, dto));
                return result.IsSucceeded ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.Fail("An error occurred: " + ex.Message));
            }
        }


        [HttpGet("{driverId}")]
        public async Task<ActionResult<Response<DriverResponseDto>>> GetDriverById(string driverId)
        {
            var result = await _mediator.Send(new GetDriverByIdQuery ( driverId ));

            if (!result.IsSucceeded)
                return NotFound(result);

            return Ok(result);
        }



        [Authorize(Roles = "Driver")]
        [HttpGet("dashboard/total-earnings")]
        public async Task<IActionResult> GetDriverTotalEarnings()
        {
            var result = await _mediator.Send(new GetDriverTotalEarningsQuery());
            return Ok(result);
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("dashboard/completion-rate")]
        public async Task<IActionResult> GetDriverCompletionRate()
        {
            var result = await _mediator.Send(new GetDriverCompletionStatsQuery());
            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("dashboard/trip-count")]
        public async Task<IActionResult> GetDriverTripCount()
        {
            var result = await _mediator.Send(new GetDriverTripCountQuery());
            return Ok(result);
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("dashboard/monthly-stats")]
        public async Task<IActionResult> GetDriverMonthlyStats()
        {
            var result = await _mediator.Send(new GetDriverMonthlyStatsQuery());
            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }




        //[HttpPost("register")]
        //public async Task<IActionResult> RegisterDriver([FromForm] RegisterDriverDto dto)
        //{
        //    try
        //    {
        //        var command = new RegisterDriverCommand(dto);
        //        var result = await _mediator.Send(command);
        //        return Ok(new { message = result });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}
    }
}
