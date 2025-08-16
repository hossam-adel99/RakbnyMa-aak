using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RakbnyMa_aak.CQRS.Reports.TripReport;
using RakbnyMa_aak.Services.Implementations;

namespace RakbnyMa_aak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminReportsController(IMediator mediator)
        {
            _mediator = mediator;

        }
        [HttpGet("trip-report")]
        public async Task<IActionResult> ExportTripReport([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _mediator.Send(new TripReportQuery(from, to));
            if (!result.IsSucceeded)
                return StatusCode(result.StatusCode, result);

            return File(
                result.Data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"TripReport_{from:yyyyMMdd}_{to:yyyyMMdd}.xlsx"
            );
        }
    }
}
