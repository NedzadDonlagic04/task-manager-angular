using Backend.API.Abstracts;
using Backend.Application.DTOs.AuthDTO;
using Backend.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers.Auth;

[Route("api/auth")]
public class AuthController(IAuthService authService) : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResultDTO>> Login(
        [FromBody] LoginCredentialsDTO loginCredentialsDTO,
        CancellationToken cancellationToken = default
    )
    {
        var loginResult = await authService.Login(loginCredentialsDTO, cancellationToken);

        return loginResult.IsFailure ? Problem(loginResult.Errors) : Ok(loginResult.Value);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status205ResetContent)]
    public async Task<ActionResult<LoginResultDTO>> Logout(
        [FromBody] RevokeRefreshTokenDTO revokeRefreshTokenDTO,
        CancellationToken cancellationToken = default
    )
    {
        var logoutResult = await authService.Logout(revokeRefreshTokenDTO, cancellationToken);

        return logoutResult.IsFailure ? Problem(logoutResult.Errors) : ResetContent();
    }
}
