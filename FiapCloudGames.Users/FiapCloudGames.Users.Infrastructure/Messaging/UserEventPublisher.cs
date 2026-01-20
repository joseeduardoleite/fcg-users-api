using FiapCloudGames.Contracts.Events;
using FiapCloudGames.Users.Application.Interfaces.Messaging;
using MassTransit;

namespace FiapCloudGames.Users.Infrastructure.Messaging;

public class UserEventPublisher(IPublishEndpoint publishEndpoint) : IUserEventPublisher
{
    public Task PublishUserCreatedAsync(Guid usuarioId, string nomeUsuario, CancellationToken cancellationToken)
    {
        var evt = new UserCreatedEvent
        {
            UsuarioId = usuarioId,
            Nome = nomeUsuario
        };

        return publishEndpoint.Publish(evt, cancellationToken);
    }
}