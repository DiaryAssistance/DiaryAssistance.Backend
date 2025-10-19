using DiaryAssistance.Application.Auth.Login;
using DiaryAssistance.Application.Auth.Models;
using DiaryAssistance.Application.Auth.Register;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DiaryAssistance.Api.Controllers;

[Route("api/auth")]
public class AuthController : BaseController
{
    [HttpPost("/login")]
    public async Task<IActionResult> Login(LoginCommand request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }

    [HttpPost("/register")]
    [ProducesResponseType<UserResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Register([FromBody] RegisterCommand request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }
}