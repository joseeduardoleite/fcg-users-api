using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace FiapCloudGames.Users.Api.Middleware;

[ExcludeFromCodeCoverage]
public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

        string message = exception.Message;

        if (exception is KeyNotFoundException) statusCode = HttpStatusCode.NotFound;
        else if (exception is UnauthorizedAccessException) statusCode = HttpStatusCode.Unauthorized;
        else if (exception is ArgumentException) statusCode = HttpStatusCode.BadRequest;

        var response = new
        {
            sucesso = false,
            mensagem = message,
            detalhes = exception.InnerException?.Message
        };

        logger.LogError(
            exception,
            "Erro ao processar requisição {Method} {Path} com status {StatusCode}",
            context.Request.Method,
            context.Request.Path,
            (int)statusCode
        );

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}