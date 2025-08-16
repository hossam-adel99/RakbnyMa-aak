using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.ApproveBookingRequest;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.RejectBookingRequest;
using RakbnyMa_aak.DTOs.BookingsDTOs.RequestsDTOs;
using RakbnyMa_aak.Models;
using System.Security.Claims;
namespace RakbnyMa_aak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Driver")]
    public class BookingApprovalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingApprovalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveBooking([FromQuery]int BooKingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            HandleBookingRequestDto dto=new HandleBookingRequestDto();
            dto.BookingId = BooKingId;
            dto.CurrentUserId = userId;

            var result = await _mediator.Send(new ApproveBookingOrchestrator(dto));
            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }
       


        [HttpPost("reject")]
        public async Task<IActionResult> RejectBooking([FromQuery] int BooKingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            HandleBookingRequestDto dto = new HandleBookingRequestDto();
            dto.BookingId = BooKingId;
            dto.CurrentUserId = userId;

            var result = await _mediator.Send(new RejectBookingOrchestrator(dto));
            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }
    }

}
