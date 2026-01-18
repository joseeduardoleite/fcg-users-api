using FiapCloudGames.Users.Api.AppServices.v1.Interfaces;
using FiapCloudGames.Users.Api.AppServices.v1;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGames.Users.Api;

[ExcludeFromCodeCoverage]
public static class ApiDependencyInjection
{
    public static IServiceCollection AddApiModule(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioAppService, UsuarioAppService>();

        return services;
    }
}