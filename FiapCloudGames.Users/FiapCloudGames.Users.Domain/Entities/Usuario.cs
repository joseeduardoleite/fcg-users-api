using FiapCloudGames.Users.Domain.Enums;

namespace FiapCloudGames.Users.Domain.Entities;

public class Usuario
{
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Senha { get; set; }
    public ERole? Role { get; set; }
    public DateTime? CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }

    public Usuario() { }

    public Usuario(string? nome, string? email, string? senha, ERole? role = null)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        Senha = senha;
        Role = role is null ? ERole.Usuario : role;
        CriadoEm = DateTime.UtcNow;
    }

    public void Atualizar(Usuario usuario)
    {
        Nome = usuario.Nome is not null ? usuario.Nome : Nome;
        Email = usuario.Email is not null ? usuario.Email : Email;
        Senha = usuario.Senha is not null ? usuario.Senha : Senha;
        AtualizadoEm = DateTime.UtcNow;
    }
}