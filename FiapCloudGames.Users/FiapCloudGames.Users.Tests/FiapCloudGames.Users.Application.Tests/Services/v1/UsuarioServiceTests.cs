using FiapCloudGames.Users.Application.Interfaces.Messaging;
using FiapCloudGames.Users.Application.Services.v1;
using FiapCloudGames.Users.Domain.Entities;
using FiapCloudGames.Users.Domain.Repositories.v1;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace FiapCloudGames.Users.Application.Tests.Services.v1;

[ExcludeFromCodeCoverage]
public class UsuarioServiceTests
{
    private readonly UsuarioService _usuarioService;

    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock = new();
    private readonly Mock<IUserEventPublisher> _userEventPublisherMock = new();

    public UsuarioServiceTests()
        => _usuarioService = new UsuarioService(
            _usuarioRepositoryMock.Object,
            _userEventPublisherMock.Object
        );

    [Fact]
    public async Task ObterUsuariosAsync_ReturnsLista()
    {
        var usuarios = new List<Usuario>
        {
            new() { Id = Guid.NewGuid(), Nome = "Duh" },
            new() { Id = Guid.NewGuid(), Nome = "Teste" }
        };

        _usuarioRepositoryMock
            .Setup(x => x.ObterUsuariosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuarios);

        var result = await _usuarioService.ObterUsuariosAsync(CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ObterUsuarioPorIdAsync_ReturnsUsuario_QuandoExiste()
    {
        var id = Guid.NewGuid();
        var usuario = new Usuario { Id = id, Nome = "Duh" };

        _usuarioRepositoryMock
            .Setup(x => x.ObterUsuarioPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        var result = await _usuarioService.ObterUsuarioPorIdAsync(id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(id, result!.Id);
    }

    [Fact]
    public async Task ObterUsuarioPorIdAsync_ReturnsNull_QuandoIdInvalido()
    {
        var result = await _usuarioService.ObterUsuarioPorIdAsync(Guid.Empty, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task ObterUsuarioPorEmailAsync_ReturnsUsuario_QuandoExiste()
    {
        var email = "duzera@emailtop.com";
        var usuario = new Usuario { Id = Guid.NewGuid(), Nome = "Duh", Email = email };

        _usuarioRepositoryMock
            .Setup(x => x.ObterUsuarioPorEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        var result = await _usuarioService.ObterUsuarioPorEmailAsync(email, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(email, result!.Email);
    }

    [Fact]
    public async Task ObterUsuarioPorEmailAsync_ReturnsNull_QuandoEmailVazio()
    {
        var result = await _usuarioService.ObterUsuarioPorEmailAsync("", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task CriarUsuarioAsync_ReturnsUsuarioCriado()
    {
        var usuario = new Usuario { Id = Guid.NewGuid(), Nome = "Novo" };

        _usuarioRepositoryMock
            .Setup(x => x.CriarUsuarioAsync(usuario, It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        _userEventPublisherMock
            .Setup(x => x.PublishUserCreatedAsync(usuario.Id, usuario.Nome!, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _usuarioService.CriarUsuarioAsync(usuario, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(usuario.Id, result.Id);
    }

    [Fact]
    public async Task EditarUsuarioAsync_ReturnsUsuarioEditado()
    {
        var id = Guid.NewGuid();
        var usuario = new Usuario { Id = id, Nome = "Editar" };

        _usuarioRepositoryMock
            .Setup(x => x.EditarUsuarioAsync(id, usuario, It.IsAny<CancellationToken>()))
            .ReturnsAsync(usuario);

        var result = await _usuarioService.EditarUsuarioAsync(id, usuario, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(id, result!.Id);
    }

    [Fact]
    public async Task DeletarUsuarioAsync_ChamaMetodoDoRepositorio()
    {
        var id = Guid.NewGuid();

        _usuarioRepositoryMock
            .Setup(x => x.DeletarUsuarioAsync(id, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        await _usuarioService.DeletarUsuarioAsync(id, CancellationToken.None);

        _usuarioRepositoryMock.Verify(x => x.DeletarUsuarioAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }
}