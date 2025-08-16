using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RakbnyMa_aak.CQRS.Features.Auth.Queries.GetUserById;
using RakbnyMa_aak.CQRS.Features.Auth.Queries.GetUserByName;
using RakbnyMa_aak.CQRS.Features.Trips.Queries.GetMyBookedTrips;
using RakbnyMa_aak.CQRS.Queries.GetUserBookings;
using RakbnyMa_aak.CQRS.Users.UpdateUserProfile;
using RakbnyMa_aak.DTOs.UserDTOs;
using RakbnyMa_aak.DTOs.UserDTOs.UpdateProfileDTOs;
using RakbnyMa_aak.GeneralResponse;
using System.Security.Claims;
using static RakbnyMa_aak.Utilities.Enums;
namespace RakbnyMa_aak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;

        }
        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromForm] RegisterUserDto dto)
        //{
        //    var result = await _mediator.Send(new RegisterUserCommand(dto));
        //    return StatusCode(result.StatusCode, result);
        //}
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<UserResponseDto>>> GetUserById(string id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery ( id ));

            if (!result.IsSucceeded)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("name")]
        public async Task<ActionResult<Response<UserResponseDto>>> GetUserByName([FromQuery]string name)
        {
            var result = await _mediator.Send(new GetUserByNameQuery(name));

            if (!result.IsSucceeded)
                return NotFound(result);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("passenger/booked-trips")]
        public async Task<IActionResult> GetMyBookedTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetMyBookedTripsQuery(page, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [Authorize(Roles = "User")]
        [HttpPatch("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new UpdateUserProfileCommand(dto,userId);
            var result = await _mediator.Send(command);

            return Ok(result);
        }


        [Authorize(Roles = "User")]
        [HttpGet("user/my-bookings")]
        public async Task<IActionResult> GetMyBookings([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] RequestStatus? status = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetMyBookingsQuery(page, pageSize, status, userId);
            var result = await _mediator.Send(query);
            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }


    }
}