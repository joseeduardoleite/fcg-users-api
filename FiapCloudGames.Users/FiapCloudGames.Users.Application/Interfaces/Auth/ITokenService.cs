namespace FiapCloudGames.Users.Application.Interfaces.Auth;

public interface ITokenService
{
    string GenerateToken(Guid id, string email, string role);
}