using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RakbnyMa_aak.CQRS.Ratings.DriverGetRatings;
using RakbnyMa_aak.CQRS.Queries.GetUserRatings;
using RakbnyMa_aak.CQRS.Features.Ratings.DriverAddRating;
using RakbnyMa_aak.CQRS.Features.Ratings.DriverUpdateRating;
using RakbnyMa_aak.CQRS.Features.Ratings.UserAddRating;
using RakbnyMa_aak.CQRS.Features.Ratings.UserUpdateRating;
using RakbnyMa_aak.CQRS.Features.Ratings.UserDeleteRating;
using RakbnyMa_aak.CQRS.Features.Ratings.DriverDeleteRating;
using System.Security.Claims;

namespace RakbnyMa_aak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RatingController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Driver")]
        [HttpGet("{driver}")]
        public async Task<IActionResult> GetDriverRatings([FromQuery] int page , [FromQuery] int pageSize)
        {
            string driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var result = await _mediator.Send(new GetDriverRatingsQuery(driverId, page, pageSize));
            return Ok(result);
        }

        [Authorize(Roles = "Usrr")]
        [HttpGet("user/{driverId}")]
        public async Task<IActionResult> GetDUserRatings([FromBody] GetUserRatingDto Filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Filter.RaterId = userId;

            var result = await _mediator.Send(new GetUserRatingsQuery(Filter));
            return Ok(result);
        }

        /// Add rating by passenger after trip ends 

        [HttpPost("add-user")]
        public async Task<IActionResult> AddRating([FromBody] UserAddRatingDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            dto.RaterId= userId;

            var result = await _mediator.Send(new UserAddRatingCommand(dto));
            return StatusCode(result.StatusCode, result);
        }

        /// Update existing rating by rater <summary>
        [Authorize(Roles = "User")]
        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateRating([FromBody] UserUpdateRatingDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            dto.RaterId = userId;

            var result = await _mediator.Send(new UserUpdateRatingCommand(dto));
            return StatusCode(result.StatusCode, result);
        }

        /// Delete existing rating <summary>
        [Authorize(Roles = "User")]
        [HttpDelete("delete-user/{ratingId}")]
        public async Task<IActionResult> DeleteRating([FromRoute] int ratingId)
        {
            string raterId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _mediator.Send(new UserDeleteRatingCommand(ratingId, raterId));
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Roles = "Driver")]
        [HttpPost("driver")]
        public async Task<IActionResult> RatePassenger([FromBody] DriverAddRatingDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            dto.RaterId = userId;

            var result = await _mediator.Send(new DriverAddRatingCommand(dto));

            if (!result.IsSucceeded)
                return BadRequest(result);

            return Ok(result);
        }


        [Authorize(Roles = "Driver")]
        [HttpPut("driver")]
        public async Task<IActionResult> UpdateDriverRating([FromBody] DriverUpdateRatingDto dto)
        {
            var result = await _mediator.Send(new DriverUpdateRatingCommand(dto));
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            dto.RaterId = userId;

            if (!result.IsSucceeded)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Driver")]
        [HttpDelete("driver")]
        public async Task<IActionResult> DeleteDriverRating([FromQuery] int ratingId )
        {
            string raterId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _mediator.Send(new DriverDeleteRatingCommand(ratingId, raterId));

            if (!result.IsSucceeded)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
