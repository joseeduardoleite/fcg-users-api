namespace FiapCloudGames.Users.Domain.Events;

public class UserCreatedEvent
{
    public Guid UsuarioId { get; init; }
    public string Nome { get; init; } = null!;
    public string? Email { get; init; } = null!;
    public DateTime CriadoEm { get; init; }
}