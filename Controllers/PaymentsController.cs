using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RakbnyMa_aak.Features.Payments;

namespace RakbnyMa_aak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("process-booking")]
        public async Task<IActionResult> ProcessBookingPayment([FromBody] ProcessBookingPayment.Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
