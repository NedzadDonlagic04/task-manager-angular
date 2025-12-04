using Backend.API.Abstracts;
using Backend.API.Extensions;
using Backend.Application.DTOs.Auth;
using Backend.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers.Auth;

[Route("api/auth")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger)
    : ApiControllerBase
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

        logger.LogInformation(
            "Login {Result}: Username='{Username}', IP={IP}",
            loginResult.IsFailure ? "Failure" : "Success",
            loginCredentialsDTO.Username,
            ClientIPAddress?.ToString()
        );

        return loginResult.IsFailure ? Problem(loginResult.Errors) : Ok(loginResult.Value);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status205ResetContent)]
    public async Task<ActionResult<LoginResultDTO>> Logout(
        [FromBody] RevokeRefreshTokenDTO revokeRefreshTokenDTO,
        CancellationToken cancellationToken = default
    )
    {
        var userIdResult = User.GetUserId();

        if (userIdResult.IsFailure)
        {
            return Problem(userIdResult.Errors);
        }

        Guid userId = userIdResult.Value;

        var logoutResult = await authService.Logout(revokeRefreshTokenDTO, cancellationToken);

        logger.LogInformation(
            "Logout {Result}: UserId='{UserId}', IP={IP}",
            logoutResult.IsFailure ? "Failure" : "Success",
            userId,
            ClientIPAddress?.ToString()
        );

        return logoutResult.IsFailure ? Problem(logoutResult.Errors) : ResetContent();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResultDTO>> Register(
        [FromBody] RegisterAccountDTO registerAccountDTO,
        CancellationToken cancellationToken = default
    )
    {
        var registerAccountResult = await authService.Register(
            registerAccountDTO,
            cancellationToken
        );

        if (registerAccountResult.IsFailure)
        {
            return Problem(registerAccountResult.Errors);
        }

        logger.LogInformation(
            "User registration Success: Username='{Username}', IP={IP}",
            registerAccountDTO.Username,
            ClientIPAddress?.ToString()
        );

        return Ok();
    }
}
