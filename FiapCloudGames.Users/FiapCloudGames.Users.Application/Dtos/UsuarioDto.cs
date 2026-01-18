using FiapCloudGames.Users.Domain.Enums;

namespace FiapCloudGames.Users.Application.Dtos;

public record UsuarioDto(
    Guid? Id,
    string? Nome,
    string? Email,
    string? Senha,
    ERole? Role = null
);