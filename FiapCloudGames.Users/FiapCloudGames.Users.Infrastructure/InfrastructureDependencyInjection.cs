using FiapCloudGames.Users.Application.Interfaces.Auth;
using FiapCloudGames.Users.Domain.Repositories.v1;
using FiapCloudGames.Users.Infrastructure.Auth;
using FiapCloudGames.Users.Infrastructure.Repositories.v1;
using FiapCloudGames.Users.Infrastructure.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FiapCloudGames.Users.Infrastructure;

[ExcludeFromCodeCoverage]
public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfraModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IUsuarioRepository, UsuarioRepository>();

        services.AddHostedService<DataSeederHostedService>();

        services.AddJwtAuthentication(configuration);

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        JwtSettings jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()!;

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey!))
                };
            });

        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}