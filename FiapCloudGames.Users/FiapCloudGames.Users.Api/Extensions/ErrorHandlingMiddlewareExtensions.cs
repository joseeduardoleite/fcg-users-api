using FiapCloudGames.Users.Api.Middleware;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGames.Users.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<ErrorHandlingMiddleware>();
}

