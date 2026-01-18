using FiapCloudGames.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGames.Users.Infrastructure.Mappings;

[ExcludeFromCodeCoverage]
public class UsuarioMap : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder
            .HasKey(u => u.Id);

        builder
            .Property(u => u.Nome)
            .HasMaxLength(100);

        builder
            .Property(u => u.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder
            .Property(u => u.Senha)
            .IsRequired();

        builder
            .Property(u => u.Role)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(u => u.CriadoEm);

        builder.Property(u => u.AtualizadoEm);
    }
}