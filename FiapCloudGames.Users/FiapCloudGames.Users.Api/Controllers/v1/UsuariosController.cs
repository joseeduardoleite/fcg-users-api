using Asp.Versioning;
using FiapCloudGames.Users.Api.AppServices.v1.Interfaces;
using FiapCloudGames.Users.Application.Dtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.Users.Api.Controllers.v1;

/// <summary>
/// API responsável pelo controle de usuários
/// </summary>
/// <param name="usuarioAppService">Serviço de aplicação de usuários</param>
/// <param name="validator">Validator de aplicação de usuários DTO</param>
[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public sealed class UsuariosController(IUsuarioAppService usuarioAppService, IValidator<UsuarioDto> validator) : FcgControllerBase
{
    /// <summary>
    /// Obtém todos os usuários (Admins - Todos, Usuários - Sem acesso)
    /// </summary>
    /// <param name="cancellationToken">Token para cancelamento da requisição</param>
    /// <response code="200">Usuários retornados com sucesso</response>
    /// <response code="204">Lista de usuários vazia</response>
    /// <response code="403">Privilégios insuficientes</response>
    /// <returns>Retorna uma lista de usuários</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAsync(CancellationToken cancellationToken)
        => ListResult(await usuarioAppService.ObterUsuariosAsync(cancellationToken));

    /// <summary>
    /// Obtém um usuário por id (Admins - Todos, Usuários - Somente o próprio)
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição</param>
    /// <response code="200">Usuário retornado com sucesso</response>
    /// <response code="403">Privilégios insuficientes</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <returns>Retorna o usuário encontrado</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> GetPorIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        UsuarioDto usuario = await usuarioAppService.ObterUsuarioPorIdAsync(id, cancellationToken);

        if (usuario is null)
            return NotFound();

        if (!IsOwnerOrAdmin((Guid)usuario.Id!))
            return Forbid();

        return Ok(usuario);
    }

    /// <summary>
    /// Obtém um usuário por e-mail (Admins - Todos, Usuários - Somente o próprio)
    /// </summary>
    /// <param name="email">E-mail do usuário</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição</param>
    /// <response code="200">Usuário retornado com sucesso</response>
    /// <response code="403">Privilégios insuficientes</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <returns>Retorna o usuário encontrado</returns>
    [HttpGet("{email}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> GetPorEmailAsync([FromRoute] string email, CancellationToken cancellationToken)
    {
        UsuarioDto usuario = await usuarioAppService.ObterUsuarioPorEmailAsync(email, cancellationToken);

        if (usuario is null)
            return NotFound();

        if (!IsOwnerOrAdmin((Guid)usuario.Id!))
            return Forbid();

        return Ok(usuario);
    }

    /// <summary>
    /// Cria um usuário (Admins - Podem criar, Usuários - Podem criar)
    /// </summary>
    /// <param name="usuarioDto">Usuário a ser criado</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição</param>
    /// <response code="201">Usuário criado com sucesso</response>
    /// <response code="400">Usuário inválido</response>
    /// <returns>Usuário criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsuarioDto>> CreateAsync([FromBody] UsuarioDto usuarioDto, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await validator.ValidateAsync(usuarioDto, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        UsuarioDto usuarioCriado = await usuarioAppService.CriarUsuarioAsync(usuarioDto, cancellationToken);

        return Created(
            uri: $"v1/usuarios/{usuarioCriado.Id}",
            value: usuarioCriado
        );
    }

    /// <summary>
    /// Atualiza um usuário (Admins - Todos, Usuários - Somente o próprio)
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <param name="usuarioDto">Dados atualizados do usuário</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição</param>
    /// <response code="200">Usuário atualizado com sucesso</response>
    /// <response code="403">Privilégios insuficientes</response>
    /// <returns>Usuário atualizado</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<UsuarioDto>> UpdateAsync([FromRoute] Guid id, [FromBody] UsuarioDto usuarioDto, CancellationToken cancellationToken)
    {
        if (!IsOwnerOrAdmin((Guid)usuarioDto.Id!))
            return Forbid();

        UsuarioDto usuarioEditado = await usuarioAppService.EditarUsuarioAsync(id, usuarioDto, cancellationToken);

        return Ok(usuarioEditado);
    }

    /// <summary>
    /// Deleta um usuário (Admins - Todos, Usuários - Sem permissão)
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <param name="cancellationToken">Token para cancelamento da requisição</param>
    /// <response code="200">Usuário deletado com sucesso</response>
    /// <response code="403">Privilégios insuficientes</response>
    /// <returns>Usuário deletado</returns>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await usuarioAppService.DeletarUsuarioAsync(id, cancellationToken);

        return Ok($"Usuário de id '{id}' deletado com sucesso!");
    }
}