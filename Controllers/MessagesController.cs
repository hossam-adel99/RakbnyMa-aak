using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RakbnyMa_aak.CQRS.Queries.GetMessagesByTripId;

namespace RakbnyMa_aak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetMessages(int tripId)
        {
            var result = await _mediator.Send(new GetMessagesByTripIdQuery(tripId));
            return StatusCode(result.StatusCode, result);
        }

    }
}
