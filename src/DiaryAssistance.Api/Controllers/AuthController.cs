using DiaryAssistance.Api.Contracts;
using DiaryAssistance.Application.Auth.Login;
using DiaryAssistance.Application.Auth.Models;
using DiaryAssistance.Application.Auth.Refresh;
using DiaryAssistance.Application.Auth.Register;
using DiaryAssistance.Application.Auth.SignOut;
using DiaryAssistance.Core.Consants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiaryAssistance.Api.Controllers;

[Route(Routes.V1.Auth.BaseAuthRoute)]
public class AuthController : BaseController
{
    [HttpPost(Routes.V1.Auth.Login)]
    [ProducesResponseType<TokensResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginCommand request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }

    [HttpPost(Routes.V1.Auth.Register)]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterCommand request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }

    [HttpPost(Routes.V1.Auth.Refresh)]
    [ProducesResponseType<TokensResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refresh([FromBody] RefreshCommand request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }

    [HttpPost(Routes.V1.Auth.SignOut)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignOut([FromBody] SignOutCommand request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }
}