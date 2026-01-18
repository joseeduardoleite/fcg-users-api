using FiapCloudGames.Users.Application.Dtos;

namespace FiapCloudGames.Users.Api.AppServices.v1.Interfaces;

public interface IUsuarioAppService
{
    Task<UsuarioTokenDto> LoginAsync(UsuarioLoginDto loginDto, CancellationToken cancellationToken);
    Task<IEnumerable<UsuarioDto>> ObterUsuariosAsync(CancellationToken cancellationToken);
    Task<UsuarioDto> ObterUsuarioPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<UsuarioDto> ObterUsuarioPorEmailAsync(string email, CancellationToken cancellationToken);
    Task<UsuarioDto> CriarUsuarioAsync(UsuarioDto usuarioDto, CancellationToken cancellationToken);
    Task<UsuarioDto> EditarUsuarioAsync(Guid id, UsuarioDto usuarioDto, CancellationToken cancellationToken);
    Task DeletarUsuarioAsync(Guid id, CancellationToken cancellationToken);
}