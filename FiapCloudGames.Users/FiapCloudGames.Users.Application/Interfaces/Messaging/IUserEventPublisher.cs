using FiapCloudGames.Users.Domain.Entities;

namespace FiapCloudGames.Users.Application.Interfaces.Messaging;

public interface IUserEventPublisher
{
    Task PublishUserCreatedAsync(Usuario usuario);
}
