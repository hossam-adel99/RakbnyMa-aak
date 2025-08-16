using MediatR;
using Microsoft.AspNetCore.Mvc;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.CQRS.Features.Cities.CreateCity;
using RakbnyMa_aak.CQRS.Features.Cities.DeleteCity;
using RakbnyMa_aak.CQRS.Features.Cities.GetAllCities;
using RakbnyMa_aak.CQRS.Features.Cities.GetCitiesByGovernorateId;
using RakbnyMa_aak.CQRS.Features.Cities.GetCityById;
using RakbnyMa_aak.CQRS.Features.Cities.GetCityByName;
using RakbnyMa_aak.CQRS.Features.Cities.UpdateCity;
using RakbnyMa_aak.DTOs.Shared;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Response<PaginatedResult<CityDto>>>> GetAllCities([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllCitiesQuery(
                new GetAllRequestDto { Page = page, PageSize = pageSize }
              ));

            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CityDto dto)
        {
            var result = await _mediator.Send(new CreateCityCommand(dto));
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CityDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(new UpdateCityCommand(dto));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCityCommand(id));
            return Ok(result);
        }

        [HttpGet("by-governorate/{governorateId}")]
        public async Task<ActionResult<Response<List<CityDto>>>> GetCitiesByGovernorateId(int governorateId)
        {
            var result = await _mediator.Send(new GetCitiesByGovernorateIdQuery(governorateId));

            if (!result.IsSucceeded)
                return NotFound(result);

            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<CityDto>>> GetCityById(int id)
        {
            var result = await _mediator.Send(new GetCityByIdQuery(id));

            if (!result.IsSucceeded)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("by-name")]
        public async Task<IActionResult> GetCityByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("City name is required.");

            var result = await _mediator.Send(new GetCityByNameQuery(name));
            if (!result.IsSucceeded)
                return NotFound(result.Message);

            return Ok(result);
        }
    }
}
