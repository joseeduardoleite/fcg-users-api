using FiapCloudGames.Users.Api.Middleware;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGames.Users.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        => builder.UseMiddleware<RequestLoggingMiddleware>();
}