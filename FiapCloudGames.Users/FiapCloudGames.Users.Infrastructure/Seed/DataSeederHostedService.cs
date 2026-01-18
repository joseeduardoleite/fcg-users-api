using FiapCloudGames.Users.Infrastructure.Repositories.v1;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGames.Users.Infrastructure.Seed;

[ExcludeFromCodeCoverage]
public sealed class DataSeederHostedService(ILogger<DataSeederHostedService> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        UsuarioRepository.Seed();

        logger.LogInformation("Dados iniciais populados com sucesso!");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}