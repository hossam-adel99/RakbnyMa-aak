using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RakbnyMa_aak.CQRS.Commands.Tracking.SendAndStoreDriverLocation;
using RakbnyMa_aak.CQRS.Queries.GetLastDriverLocation;
using RakbnyMa_aak.CQRS.Queries.GetTripCoordinates;
using RakbnyMa_aak.DTOs.TripTrackingDTOs;
using System.Security.Claims;

namespace RakbnyMa_aak.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TrackingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetLocation(int tripId)
        {
            var response = await _mediator.Send(new GetLastDriverLocationQuery(tripId));
            return StatusCode(response.StatusCode, response);
        }


        [HttpPost("send-location")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> SendLocation([FromBody] SendLocationDto dto)
        {
            var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _mediator.Send(new SendAndStoreDriverLocationCommand(dto.TripId, dto.Lat, dto.Lng, driverId));
            return StatusCode(response.StatusCode, response);
        }


    }

}
