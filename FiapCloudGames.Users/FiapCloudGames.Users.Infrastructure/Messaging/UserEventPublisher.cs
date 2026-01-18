using FiapCloudGames.Users.Application.Interfaces.Messaging;
using FiapCloudGames.Users.Domain.Entities;
using FiapCloudGames.Users.Domain.Events;
using MassTransit;

namespace FiapCloudGames.Users.Infrastructure.Messaging;

public class UserEventPublisher(IPublishEndpoint publishEndpoint) : IUserEventPublisher
{
    public Task PublishUserCreatedAsync(Usuario usuario)
    {
        var evt = new UserCreatedEvent
        {
            UsuarioId = usuario.Id,
            Nome = usuario.Nome!,
            Email = usuario.Email!,
            CriadoEm = usuario.CriadoEm ?? DateTime.UtcNow
        };

        return publishEndpoint.Publish(evt);
    }
}