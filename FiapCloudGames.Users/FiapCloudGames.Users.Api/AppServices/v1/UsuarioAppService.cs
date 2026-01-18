using AutoMapper;
using FiapCloudGames.Users.Api.AppServices.v1.Interfaces;
using FiapCloudGames.Users.Application.Dtos;
using FiapCloudGames.Users.Application.Interfaces.Auth;
using FiapCloudGames.Users.Domain.Entities;
using FiapCloudGames.Users.Domain.Services.v1;

namespace FiapCloudGames.Users.Api.AppServices.v1;

public sealed class UsuarioAppService(
    IUsuarioService usuarioService,
    IMapper mapper,
    ITokenService tokenService) : IUsuarioAppService
{
    public async Task<UsuarioTokenDto> LoginAsync(UsuarioLoginDto loginDto, CancellationToken cancellationToken)
    {
        Usuario? usuario = await usuarioService.ObterUsuarioPorEmailAsync(loginDto.Email!, cancellationToken);

        if (usuario is null || usuario.Senha != loginDto.Senha)
            throw new UnauthorizedAccessException("Credenciais inválidas");

        string token = tokenService.GenerateToken(usuario.Id, usuario.Email!, usuario.Role.ToString()!);

        return new UsuarioTokenDto(
            Id: usuario.Id,
            Email: usuario.Email,
            Role: usuario.Role.ToString(),
            Token: token
        );
    }

    public async Task<IEnumerable<UsuarioDto>> ObterUsuariosAsync(CancellationToken cancellationToken)
        => mapper.Map<IEnumerable<UsuarioDto>>(await usuarioService.ObterUsuariosAsync(cancellationToken));

    public async Task<UsuarioDto> ObterUsuarioPorIdAsync(Guid id, CancellationToken cancellationToken)
        => mapper.Map<UsuarioDto>(await usuarioService.ObterUsuarioPorIdAsync(id, cancellationToken));

    public async Task<UsuarioDto> ObterUsuarioPorEmailAsync(string email, CancellationToken cancellationToken)
        => mapper.Map<UsuarioDto>(await usuarioService.ObterUsuarioPorEmailAsync(email, cancellationToken));

    public async Task<UsuarioDto> CriarUsuarioAsync(UsuarioDto usuarioDto, CancellationToken cancellationToken)
        => mapper.Map<UsuarioDto>(await usuarioService.CriarUsuarioAsync(mapper.Map<Usuario>(usuarioDto), cancellationToken));

    public async Task<UsuarioDto> EditarUsuarioAsync(Guid id, UsuarioDto usuarioDto, CancellationToken cancellationToken)
        => mapper.Map<UsuarioDto>(await usuarioService.EditarUsuarioAsync(id, mapper.Map<Usuario>(usuarioDto), cancellationToken));

    public async Task DeletarUsuarioAsync(Guid id, CancellationToken cancellationToken)
        => await usuarioService.DeletarUsuarioAsync(id, cancellationToken);
}