using FiapCloudGames.Users.Application.Interfaces.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace FiapCloudGames.Users.Infrastructure.Auth;

[ExcludeFromCodeCoverage]
public sealed class TokenService(IOptions<JwtSettings> settings) : ITokenService
{
    private readonly JwtSettings _settings = settings.Value;

    public string GenerateToken(Guid id, string email, string role)
    {
        List<Claim> claims =
        [
            new Claim(type: JwtRegisteredClaimNames.Sub, value: id.ToString()),
            new Claim(type: ClaimTypes.NameIdentifier, value: id.ToString()),
            new Claim(type: ClaimTypes.Email, value: email),
            new Claim(type: ClaimTypes.Role, value: role)
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_settings.SecretKey));
        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}