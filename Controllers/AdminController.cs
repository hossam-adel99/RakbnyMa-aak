using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RakbnyMa_aak.CQRS.Features.Admin.ApproveDriver;
using RakbnyMa_aak.CQRS.Features.Admin.RejectDriver;
using RakbnyMa_aak.CQRS.Features.Auth.Queries.GetPendingDrivers;

namespace RakbnyMa_aak.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{driverId}/approve")]
        public async Task<IActionResult> ApproveDriver(string driverId)
        {
            var result = await _mediator.Send(new ApproveDriverCommand (driverId ));
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{driverId}/reject")]
        public async Task<IActionResult> RejectDriver(string driverId, [FromBody] string? reason = null)
        {
            var result = await _mediator.Send(new RejectDriverCommand(driverId, reason));
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("pending-register")]
        public async Task<IActionResult> GetPendingDrivers()
        {
            var result = await _mediator.Send(new GetPendingDriversQuery());
            return StatusCode(result.StatusCode, result);
        }
    }
}
