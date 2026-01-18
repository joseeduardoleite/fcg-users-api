using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Text.Json;

namespace FiapCloudGames.Users.Api.Middleware;

[ExcludeFromCodeCoverage]
public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        await next(context);

        stopwatch.Stop();

        var log = new
        {
            Metodo = context.Request.Method,
            Rota = context.Request.Path,
            context.Response.StatusCode,
            DuracaoMs = stopwatch.ElapsedMilliseconds
        };

        logger.LogInformation("Requisição processada: {Log}", JsonSerializer.Serialize(log));
    }
}