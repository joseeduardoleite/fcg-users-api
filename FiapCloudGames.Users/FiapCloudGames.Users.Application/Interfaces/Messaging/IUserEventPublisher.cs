namespace FiapCloudGames.Users.Application.Interfaces.Messaging;

public interface IUserEventPublisher
{
    Task PublishUserCreatedAsync(Guid usuarioId, string nomeUsuario, CancellationToken cancellationToken);
}
