using Asp.Versioning;
using FiapCloudGames.Users.Api.AppServices.v1.Interfaces;
using FiapCloudGames.Users.Api.Examples;
using FiapCloudGames.Users.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace FiapCloudGames.Users.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public sealed class AuthController(IUsuarioAppService usuarioAppService) : ControllerBase
{
    [HttpPost("[action]")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UsuarioTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerRequestExample(typeof(UsuarioLoginDto), typeof(UsuarioLoginRequestExample))]
    public async Task<IActionResult> LoginAsync([FromBody] UsuarioLoginDto loginDto, CancellationToken cancellationToken)
    {
        try
        {
            UsuarioTokenDto token = await usuarioAppService.LoginAsync(loginDto, cancellationToken);

            return Ok(token);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized($"Erro inesperado: {ex.Message}");
        }
    }
}