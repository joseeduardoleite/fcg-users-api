using AutoMapper;
using FiapCloudGames.Users.Application.Dtos;
using FiapCloudGames.Users.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGames.Users.Application.Mappers;

[ExcludeFromCodeCoverage]
public sealed class UsuarioMapper : Profile
{
    public UsuarioMapper()
        => CreateMap<Usuario, UsuarioDto>().ReverseMap();
}