using FiapCloudGames.Users.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGames.Users.Infrastructure.Seed;

[ExcludeFromCodeCoverage]
public static class UsuarioSeed
{
    public static IEnumerable<Usuario> GetUsuarios()
        => new List<Usuario>()
        {
            new(
                nome: "José",
                email: "jose.fcg@gmail.com",
                senha: "Jose@1234",
                role: Domain.Enums.ERole.Admin
            ),
            new(
                nome: "Admin",
                email: "admin.fcg@gmail.com",
                senha: "Admin@1234",
                role: Domain.Enums.ERole.Admin
            ),
            new(
                nome: "Eduardo",
                email: "eduardo.fcg@gmail.com",
                senha: "Eduardo@1234"
            ),
            new(
                nome: "João",
                email: "joão.fcg@gmail.com",
                senha: "Joao@1234"
            ),
            new(
                nome: "Maria",
                email: "maria.fcg@gmail.com",
                senha: "Maria@1234"
            )
        };
}