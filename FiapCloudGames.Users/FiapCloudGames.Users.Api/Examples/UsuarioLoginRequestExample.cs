using FiapCloudGames.Users.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGames.Users.Api.Examples;

[ExcludeFromCodeCoverage]
public sealed class UsuarioLoginRequestExample : IExamplesProvider<UsuarioLoginDto>
{
    public UsuarioLoginDto GetExamples() => new(
        Email: "admin.fcg@gmail.com",
        Senha: "Admin@1234"
    );
}