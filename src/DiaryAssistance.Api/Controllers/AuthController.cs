using DiaryAssistance.Application.Auth.Login;
using Microsoft.AspNetCore.Mvc;

namespace DiaryAssistance.Api.Controllers;

[Route("api/auth")]
public class AuthController : BaseController
{
    [HttpPost("/login")]
    public async Task<IActionResult> Login(LoginCommand request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }
}