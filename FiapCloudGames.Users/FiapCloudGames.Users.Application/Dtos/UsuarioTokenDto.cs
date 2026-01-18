namespace FiapCloudGames.Users.Application.Dtos;

public record UsuarioTokenDto(Guid? Id, string? Email, string? Role, string? Token);