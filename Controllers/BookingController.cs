using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.CancelBookingByPassenger;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Orchestrators.BookTripRequest;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Orchestrators.UpdateBookingOrchestrator;
using RakbnyMa_aak.CQRS.Queries.Driver.GetPendingBooking;
using RakbnyMa_aak.CQRS.Queries.Driver.GetTripConfirmedBookings;
using RakbnyMa_aak.DTOs.BookingsDTOs.Requests;
using RakbnyMa_aak.DTOs.BookingsDTOs.RequestsDTOs;
using System.Security.Claims;
namespace RakbnyMa_aak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BookingController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Authorize(Roles = "User")]
        [HttpPost("book")]
        public async Task<IActionResult> BookTrip([FromBody] BookTripRequestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _mediator.Send(new BookTripRequestOrchestrator(dto, userId));
            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "User")]
        [HttpPut("update-booking")]
        public async Task<IActionResult> UpdateBooking([FromBody] UpdateBookingRequestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new UpdateBookingOrchestrator(dto, userId );
            var result = await _mediator.Send(command);
            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }


        [Authorize(Roles = "User")]
        [HttpDelete("cancel")]
        public async Task<IActionResult> CancelBooking([FromQuery] int BookingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _mediator.Send(new CancelBookingByPassengerCommand(BookingId, userId));
            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }


        [Authorize(Roles = "Driver")]
        [HttpGet("driver/trip/{tripId}/pending-bookings")]
        public async Task<IActionResult> GetTripPendingBookings(int tripId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetTripPendingBookingsQuery(tripId, page, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [Authorize(Roles = "Driver")]
        [HttpGet("driver/trip/{tripId}/confirmed-bookings")]
        public async Task<IActionResult> GetTripConfirmedBookings(int tripId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetTripConfirmedBookingsQuery(tripId, page, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }




    }
}
