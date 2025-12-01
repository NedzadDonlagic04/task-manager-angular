using Backend.API.Abstracts;
using Backend.Application.DTOs.AuthDTO;
using Backend.Application.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers.Users;

[Route("api/auth")]
public class AuthController(IUserService userService) : ApiControllerBase
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
        var loginResult = await userService.Login(loginCredentialsDTO, cancellationToken);

        return loginResult.IsFailure ? Problem(loginResult.Errors) : Ok(loginResult.Value);
    }
}
