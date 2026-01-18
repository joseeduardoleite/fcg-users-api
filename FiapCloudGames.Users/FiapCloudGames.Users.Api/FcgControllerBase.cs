using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FiapCloudGames.Users.Api;

/// <summary>
/// Controller base que fornece recursos comuns para todas as controllers da API.
/// Inclui informações do usuário autenticado, respostas padronizadas e tipos de retorno para Swagger.
/// </summary>
/// <response code="200">Dados obtidos com sucesso</response>
/// <response code="201">Dados criados com sucesso</response>
/// <response code="204">Dados vazios</response>
/// <response code="400">Requisição inválida</response>
/// <response code="401">Não autorizado</response>
/// <response code="403">Privilégios insuficientes</response>
/// <response code="404">Não encontrado</response>
/// <response code="500">Erro interno</response>
[Authorize]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ExcludeFromCodeCoverage]
public abstract class FcgControllerBase : ControllerBase
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    /// <summary>
    /// Id do usuário extraído do claim ClaimTypes.NameIdentifier (padrão: Guid em string)
    /// </summary>
    protected string? UsuarioId => User.FindFirstValue(ClaimTypes.NameIdentifier)
                                        ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

    /// <summary>
    /// E-mail do usuário extraído dos claims
    /// </summary>
    protected string? UsuarioEmail => User.FindFirstValue(ClaimTypes.Email)
                                        ?? User.FindFirstValue("email");

    /// <summary>
    /// Role do usuário (ex: "Admin", "Usuario")
    /// </summary>
    protected string? UsuarioRole => User.FindFirstValue(ClaimTypes.Role);

    /// <summary>
    /// Retorna o UsuarioId como Guid se possível.
    /// Caso contrário null
    /// </summary>
    protected Guid? GetUsuarioIdGuid()
    {
        if (string.IsNullOrWhiteSpace(UsuarioId))
            return null;

        return Guid.TryParse(UsuarioId, out var g) ? g : null;
    }

    /// <summary>
    /// Indica se o usuário autenticado tem a role Admin
    /// </summary>
    protected bool IsAdmin()
        => string.Equals(UsuarioRole, "Admin", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Verifica se o usuário autenticado é o proprietário (por Guid) ou Admin
    /// </summary>
    internal bool IsOwnerOrAdmin(Guid resourceOwnerId)
    {
        var uid = GetUsuarioIdGuid();
        return (uid.HasValue && uid.Value == resourceOwnerId) || IsAdmin();
    }

    /// <summary>
    /// Retorna uma resposta padronizada para listas.
    /// </summary>
    protected ActionResult ListResult<T>(IEnumerable<T> dados)
        => dados is null || !dados.Any() ? NoContent() : Ok(dados);

    /// <summary>
    /// Retorna uma resposta JSON padronizada para objetos únicos.
    /// </summary>
    protected ActionResult Result<T>(T? dado)
        => dado is null
            ? NotFound(new { sucesso = false, mensagem = "Não encontrado" })
            : Content(JsonSerializer.Serialize(dado, jsonSerializerOptions), "application/json");
}