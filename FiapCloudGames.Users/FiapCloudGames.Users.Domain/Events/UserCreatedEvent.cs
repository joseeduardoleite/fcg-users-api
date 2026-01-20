namespace FiapCloudGames.Contracts.Events;

public class UserCreatedEvent
{
    public Guid UsuarioId { get; init; }
    public string? Nome { get; set; }
}