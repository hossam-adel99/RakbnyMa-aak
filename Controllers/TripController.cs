using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RakbnyMa_aak.CQRS.Features.Trip.Commands.CreateTrip;
using RakbnyMa_aak.CQRS.Features.Trip.Commands.EndTripByDriver;
using RakbnyMa_aak.CQRS.Features.Trip.Commands.EndTripByPassenger;
using RakbnyMa_aak.CQRS.Features.Trip.Commands.StartTripByDriver;
using RakbnyMa_aak.CQRS.Features.Trip.Commands.StartTripByPassenger;
using RakbnyMa_aak.CQRS.Features.Trip.Orchestrators.UpdateTrip;
using RakbnyMa_aak.CQRS.Features.Trip.Queries.GetDriverTripBookings;
using RakbnyMa_aak.CQRS.Features.Trip.Queries.GetMyTrips;
using RakbnyMa_aak.CQRS.Features.Trips.Queries.GetAllTrips;
using RakbnyMa_aak.CQRS.Features.Trips.Queries.GetScheduledForDriver;
using RakbnyMa_aak.CQRS.Features.Trips.Queries.GetScheduledTrips;
using RakbnyMa_aak.CQRS.Features.Trips.Queries.GetTripByBookingId;
using RakbnyMa_aak.CQRS.Features.Trips.Queries.GetTripById;
using RakbnyMa_aak.CQRS.Features.Trips.Queries.GetTripPassengers;
using RakbnyMa_aak.CQRS.Queries.Driver.GetPendingBookingsForDriver;
using RakbnyMa_aak.CQRS.Queries.GetTripCoordinates;
using RakbnyMa_aak.CQRS.Trips.Delete_Trip;
using RakbnyMa_aak.DTOs.TripDTOs.RequestsDTOs;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using System.Security.Claims;

namespace RakbnyMa_aak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {

        private readonly IMediator _mediator;

        public TripController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Driver")]
        [HttpPost]
        public async Task<IActionResult> CreateTrip([FromBody] TripRequestDto dto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var command = new CreateTripOrchestrator(dto, currentUserId);
            var result = await _mediator.Send(command);

            if (!result.IsSucceeded)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> UpdateTrip(int id, [FromBody] TripRequestDto dto)
        {

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _mediator.Send(new UpdateTripOrchestrator
            (
                 id,
                 currentUserId,
                 dto


            ));

            if (!result.IsSucceeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _mediator.Send(new DeleteTripCommand
            (
                id,
                currentUserId
            ));


            if (!result.IsSucceeded)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpGet("my-trips")]
        [Authorize]
        public async Task<ActionResult<Response<PaginatedResult<TripRequestDto>>>> GetMyTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetMyTripsQuery(page, pageSize));
            return StatusCode(result.StatusCode, result);
        }



        [Authorize(Roles = "Driver")]
        [HttpGet("bookings")]
        public async Task<IActionResult> GetBookingsForDriver()
        {
            var driverUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = new GetDriverTripBookingsQuery(driverUserId);

            var result = await _mediator.Send(query);

            if (!result.IsSucceeded)
                return BadRequest(result.Message);

            return Ok(result);
        }



        [Authorize(Roles = "Driver")]
        [HttpPost("start-by-driver")]
        public async Task<IActionResult> StartTripByDriver([FromQuery] int tripId)
        {
            var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new StartTripByDriverCommand(tripId, driverId);

            var result = await _mediator.Send(command);

            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }


        [Authorize(Roles = "Driver")]
        [HttpPost("end-by-driver")]
        public async Task<IActionResult> EndTripByDriver([FromQuery] int tripId)
        {
            var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new EndTripByDriverOrchestrator(tripId, driverId);

            var result = await _mediator.Send(command);

            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }


        [Authorize(Roles = "User")]
        [HttpPost("start-by-passenger")]
        public async Task<IActionResult> StartTripByPassenger([FromQuery] int bookingId)
        {
            var passengerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new StartTripByPassengerCommand(bookingId, passengerId);

            var result = await _mediator.Send(command);

            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "User")]
        [HttpPost("end-by-passenger")]
        public async Task<IActionResult> EndTripByPassenger([FromQuery] int bookingId)
        {
            var passengerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new EndTripByPassengerCommand(bookingId, passengerId);

            var result = await _mediator.Send(command);

            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("AllTrips")]
        public async Task<ActionResult<PaginatedResult<TripResponseDto>>> GetAllTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllTripsQuery(page, pageSize));
            return Ok(result);
        }


        [Authorize(Roles = "Admin,User")]
        [HttpGet("all-scheduled-trips")]
        public async Task<IActionResult> GetScheduledTrips([FromQuery] ScheduledTripRequestDto filter)
        {
            var query = new GetScheduledTripsQuery(filter);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // [Authorize(Roles = "Driver")]
        //public async Task<ActionResult<Response<PaginatedResult<TripResponseDto>>>> GetScheduledTripsForDriver(
        //         [FromBody] ScheduledTripRequestDto filterDto)


        
        [Authorize(Roles = "Driver")]
        [HttpGet("driver/scheduled")]
        public async Task<ActionResult> GetScheduledTripsForDriver([FromQuery] ScheduledTripRequestDto filter)
        {
            var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(driverId))
              return Unauthorized("Unauthorized access - missing driver ID");

            var query = new GetScheduledForDriverQuery(filter, driverId);
            var result = await _mediator.Send(query);
            return Ok(result);
            
        }

    
        [Authorize(Roles = "Admin,Driver,User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripById(int id)
        {
            var result = await _mediator.Send(new GetTripByIdQuery(id));
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("pending-bookings")]
        public async Task<IActionResult> GetPendingBookingsForDriver()
        {
            var driverUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = new GetPendingBookingsForDriverQuery(driverUserId);

            var result = await _mediator.Send(query);

            if (!result.IsSucceeded)
                return BadRequest(result.Message);

            return Ok(result);
        }


        [Authorize(Roles = "Driver")]
        [HttpGet("{tripId}/passengers")]
        public async Task<IActionResult> GetTripPassengers(int tripId)
        {
            var driverUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = new GetTripPassengersQuery(tripId, driverUserId);
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("by-booking/{bookingId}")]
        public async Task<IActionResult> GetTripByBookingId(int bookingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _mediator.Send(new GetTripByBookingIdQuery(bookingId, userId));
            return Ok(result);
        }



        [HttpGet("coordinates/{tripId}")]
        [Authorize(Roles = "Driver,Admin")]
        public async Task<IActionResult> GetTripCoordinates(int tripId)
        {
            var result = await _mediator.Send(new GetTripCoordinatesQuery(tripId));
            return StatusCode(result.StatusCode, result);
        }


    }
}
