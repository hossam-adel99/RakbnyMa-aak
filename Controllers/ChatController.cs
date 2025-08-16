using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RakbnyMa_aak.CQRS.Queries.GetMessagesByTripId;
using RakbnyMa_aak.CQRS.Shared.SendMessage;
using RakbnyMa_aak.GeneralResponse;
using System.Security.Claims;

namespace RakbnyMa_aak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("send")]
        public async Task<ActionResult<Response<string>>> SendMessage([FromBody] SendMessageDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var command = new SendChatMessageCommand(userId, dto);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("trip/{tripId}/messages")]
        [Authorize]
        public async Task<IActionResult> GetMessagesByTripId(int tripId)
        {
            var result = await _mediator.Send(new GetMessagesByTripIdQuery(tripId));
            return Ok(result);
        }

    }
}
