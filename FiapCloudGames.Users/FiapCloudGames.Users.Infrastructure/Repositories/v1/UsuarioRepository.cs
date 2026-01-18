using FiapCloudGames.Users.Domain.Entities;
using FiapCloudGames.Users.Domain.Repositories.v1;
using FiapCloudGames.Users.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGames.Users.Infrastructure.Repositories.v1;

[ExcludeFromCodeCoverage]
public sealed class UsuarioRepository(AppDbContext context) : IUsuarioRepository
{
    public async Task<IEnumerable<Usuario>> ObterUsuariosAsync(CancellationToken cancellationToken)
        => await context.Usuarios.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Usuario?> ObterUsuarioPorIdAsync(Guid id, CancellationToken cancellationToken)
        => await context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);

    public async Task<Usuario?> ObterUsuarioPorEmailAsync(string email, CancellationToken cancellationToken)
        => await context.Usuarios
            .FirstOrDefaultAsync(usuario =>
                EF.Functions.Like(usuario.Email, email),
                cancellationToken
            );

    public async Task<Usuario> CriarUsuarioAsync(Usuario usuario, CancellationToken cancellationToken)
    {
        Usuario usuarioCriado = new(
            nome: usuario.Nome,
            email: usuario.Email,
            senha: usuario.Senha
        );

        await context.Usuarios.AddAsync(usuarioCriado, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return usuarioCriado;
    }

    public async Task<Usuario> EditarUsuarioAsync(Guid id, Usuario usuario, CancellationToken cancellationToken)
    {
        Usuario? usuarioParaAtualizar = await ObterUsuarioPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Usuário não encontrado");

        usuarioParaAtualizar.Atualizar(usuario);

        context.Usuarios.Update(usuarioParaAtualizar);
        await context.SaveChangesAsync(cancellationToken);

        return usuarioParaAtualizar;
    }

    public async Task DeletarUsuarioAsync(Guid id, CancellationToken cancellationToken)
    {
        Usuario? usuarioParaDeletar = await ObterUsuarioPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Usuário não encontrado");

        context.Usuarios.Remove(usuarioParaDeletar);
        await context.SaveChangesAsync(cancellationToken);
    }
}