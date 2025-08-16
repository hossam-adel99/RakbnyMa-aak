using MediatR;
using Microsoft.AspNetCore.Mvc;
using RakbnyMa_aak.CQRS.Features.Admin.Login;
using RakbnyMa_aak.CQRS.Features.Auth.Commands.LoginDriver;
using RakbnyMa_aak.CQRS.Features.Auth.Commands.LoginUser;
using RakbnyMa_aak.CQRS.Features.Auth.Commands.RegisterDriver;
using RakbnyMa_aak.CQRS.Features.Auth.Commands.RegisterUser;
using RakbnyMa_aak.DTOs.Auth.RequestDTOs;
using RakbnyMa_aak.DTOs.Auth.Requests;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;


    [HttpPost("admin/login")]
    public async Task<IActionResult> LoginAdmin([FromBody] LoginRequestDto dto)
    {
        var result = await _mediator.Send(new LoginAdminCommand(dto));
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("user/register")]
    public async Task<IActionResult> RegisterUser([FromForm] RegisterUserRequestDto dto)
    {
        var result = await _mediator.Send(new RegisterUserCommand(dto));
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("driver/register")]
    public async Task<IActionResult> RegisterDriver([FromForm] RegisterDriverRequestDto dto)
    {
        var result = await _mediator.Send(new RegisterDriverCommand(dto));
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("user/login")]
    public async Task<IActionResult> LoginUser(LoginRequestDto dto)
    {
        var result = await _mediator.Send(new LoginUserCommand(dto));
        return result.IsSucceeded ? Ok(result) : BadRequest(result);
    }

    [HttpPost("driver/login")]
    public async Task<IActionResult> LoginDriver(LoginRequestDto dto)
    {
        var result = await _mediator.Send(new LoginDriverCommand(dto));
        return result.IsSucceeded ? Ok(result) : BadRequest(result);
    }
}
