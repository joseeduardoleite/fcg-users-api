using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGames.Users.Infrastructure.Auth;

[ExcludeFromCodeCoverage]
public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public double ExpirationMinutes { get; set; } = 60.0;
}