using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RakbnyMa_aak.CQRS.Features.Governorates;
using RakbnyMa_aak.CQRS.Features.Governorates.CreateGovernorate;
using RakbnyMa_aak.CQRS.Features.Governorates.DeleteGovernorate;
using RakbnyMa_aak.CQRS.Features.Governorates.GetAllGovernorates;
using RakbnyMa_aak.CQRS.Features.Governorates.GetGovernorateById;
using RakbnyMa_aak.CQRS.Features.Governorates.GetGovernorateByName;
using RakbnyMa_aak.CQRS.Features.Governorates.RestoreGovernorate;
using RakbnyMa_aak.CQRS.Features.Governorates.UpdateGovernorate;
using RakbnyMa_aak.DTOs.Shared;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernorateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GovernorateController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<Response<PaginatedResult<GovernorateDto>>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllGovernoratesQuery(
                new GetAllRequestDto { Page = page, PageSize = pageSize }
            ));
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GovernorateDto dto)
        {
            var result = await _mediator.Send(new CreateGovernorateCommand(dto));
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GovernorateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(new UpdateGovernorateCommand(dto));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteGovernorateCommand(id));
            return Ok(result);
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await _mediator.Send(new RestoreGovernorateCommand(id));
            return result.IsSucceeded ? Ok(result) : NotFound(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<GovernorateDto>>> GetGovernorateById(int id)
        {
            var result = await _mediator.Send(new GetGovernorateByIdQuery(id));

            if (!result.IsSucceeded)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetGovernorateByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(Response<string>.Fail("Governorate name is required."));

            var result = await _mediator.Send(new GetGovernorateByNameQuery(name));
            if (!result.IsSucceeded)
                return NotFound(result);

            return Ok(result);
        }

    }
}
