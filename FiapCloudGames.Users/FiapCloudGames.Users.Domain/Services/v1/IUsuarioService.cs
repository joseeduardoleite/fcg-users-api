using FiapCloudGames.Users.Domain.Entities;

namespace FiapCloudGames.Users.Domain.Services.v1;

public interface IUsuarioService
{
    Task<IEnumerable<Usuario>> ObterUsuariosAsync(CancellationToken cancellationToken);
    Task<Usuario?> ObterUsuarioPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Usuario?> ObterUsuarioPorEmailAsync(string email, CancellationToken cancellationToken);
    Task<Usuario> CriarUsuarioAsync(Usuario usuario, CancellationToken cancellationToken);
    Task<Usuario?> EditarUsuarioAsync(Guid id, Usuario usuario, CancellationToken cancellationToken);
    Task DeletarUsuarioAsync(Guid id, CancellationToken cancellationToken);
}