using FiapCloudGames.Users.Domain.Entities;
using FiapCloudGames.Users.Domain.Repositories.v1;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGames.Users.Infrastructure.Repositories.v1;

[ExcludeFromCodeCoverage]
public sealed class UsuarioRepository : IUsuarioRepository
{

    private static readonly List<Usuario> _usuarios = [];

    public static void Seed()
    {
        if (_usuarios.Count != 0)
            return;

        _usuarios.AddRange(new List<Usuario>()
        {
            new(
                nome: "José",
                email: "jose.fgc@gmail.com",
                senha: "Jose@1234",
                role: Domain.Enums.ERole.Admin
            ),
            new(
                nome: "Admin",
                email: "admin.fgc@gmail.com",
                senha: "Admin@1234",
                role: Domain.Enums.ERole.Admin
            ),
            new(
                nome: "Eduardo",
                email: "eduardo.fgc@gmail.com",
                senha: "Eduardo@1234"
            ),
            new(
                nome: "João",
                email: "joão.fgc@gmail.com",
                senha: "Joao@1234"
            ),
            new(
                nome: "Maria",
                email: "maria.fgc@gmail.com",
                senha: "Maria@1234"
            )
        });
    }

    public async Task<IEnumerable<Usuario>> ObterUsuariosAsync(CancellationToken cancellationToken)
        => await Task.FromResult(_usuarios);

    public async Task<Usuario?> ObterUsuarioPorIdAsync(Guid id, CancellationToken cancellationToken)
        => await Task.FromResult(_usuarios.FirstOrDefault(usuario => usuario.Id == id));

    public async Task<Usuario?> ObterUsuarioPorEmailAsync(string email, CancellationToken cancellationToken)
        => await Task.FromResult(_usuarios.FirstOrDefault(usuario => usuario.Email!.Equals(email, StringComparison.OrdinalIgnoreCase)));

    public async Task<Usuario> CriarUsuarioAsync(Usuario usuario, CancellationToken cancellationToken)
    {
        Usuario usuarioCriado = new(
            nome: usuario.Nome,
            email: usuario.Email,
            senha: usuario.Senha
        );

        _usuarios.Add(usuarioCriado);

        return await Task.FromResult(usuarioCriado);
    }

    public async Task<Usuario> EditarUsuarioAsync(Guid id, Usuario usuario, CancellationToken cancellationToken)
    {
        Usuario? usuarioParaAtualizar = await ObterUsuarioPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Usuário não encontrado");

        usuarioParaAtualizar.Atualizar(usuario);

        return await Task.FromResult(usuarioParaAtualizar);
    }

    public async Task DeletarUsuarioAsync(Guid id, CancellationToken cancellationToken)
    {
        Usuario? usuarioParaDeletar = await ObterUsuarioPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Usuário não encontrado");

        _usuarios.Remove(usuarioParaDeletar);
    }
}