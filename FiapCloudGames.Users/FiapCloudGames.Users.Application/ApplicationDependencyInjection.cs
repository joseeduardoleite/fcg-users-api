using FiapCloudGames.Users.Application.Mappers;
using FiapCloudGames.Users.Application.Services.v1;
using FiapCloudGames.Users.Domain.Services.v1;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGames.Users.Application;

[ExcludeFromCodeCoverage]
public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioService, UsuarioService>();

        services.AddAutoMapper(mapperConfigurationExpression =>
        {
            mapperConfigurationExpression.AddProfile(typeof(UsuarioMapper));
        });

        return services;
    }
}