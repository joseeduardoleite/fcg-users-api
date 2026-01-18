using FiapCloudGames.Users.Api.AppServices.v1.Interfaces;
using FiapCloudGames.Users.Api.Controllers.v1;
using FiapCloudGames.Users.Application.Dtos;
using FiapCloudGames.Users.Domain.Enums;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace FiapCloudGames.Users.Api.Tests.Controllers.v1;

[ExcludeFromCodeCoverage]
public class UsuariosControllerTests
{
    private readonly UsuariosController _controller;

    private readonly Mock<IUsuarioAppService> _usuarioAppServiceMock = new();
    private readonly Mock<IValidator<UsuarioDto>> _usuarioValidatorMock = new();

    public UsuariosControllerTests()
    {
        _controller = new UsuariosController(_usuarioAppServiceMock.Object, _usuarioValidatorMock.Object);

        ClaimsPrincipal user = new(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, "Admin")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetAsync_ReturnsListaUsuarios()
    {
        var usuarios = new List<UsuarioDto>
        {
            new(Guid.NewGuid(), "Eduardo", "eduardo@test.com", "Eduardo@1234", ERole.Usuario),
            new(Guid.NewGuid(), "Francisco", "francisco@test.com", "Francisco@1234", ERole.Admin)
        };

        _usuarioAppServiceMock.Setup(s => s.ObterUsuariosAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(usuarios);

        var result = await _controller.GetAsync(CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<UsuarioDto>>(okResult.Value);

        Assert.Equal(2, ((List<UsuarioDto>)returnValue).Count);
    }

    [Fact]
    public async Task GetPorIdAsync_UsuarioNaoEncontrado_ReturnsNotFound()
    {
        _usuarioAppServiceMock.Setup(s => s.ObterUsuarioPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((UsuarioDto)null!);

        var result = await _controller.GetPorIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetPorIdAsync_UsuarioEncontrado_ReturnsOk()
    {
        var id = Guid.NewGuid();
        var usuario = new UsuarioDto(id, "Eduardo", "eduardo@test.com", "Eduardo@1234", ERole.Usuario);

        _usuarioAppServiceMock.Setup(s => s.ObterUsuarioPorIdAsync(id, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(usuario);

        var result = await _controller.GetPorIdAsync(id, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<UsuarioDto>(okResult.Value);

        Assert.Equal(id, returnValue.Id);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreated()
    {
        var usuarioDto = new UsuarioDto(null, "Eduardo", "eduardo@test.com", "Eduardo@1234", ERole.Usuario);
        var createdUsuario = new UsuarioDto(Guid.NewGuid(), "Francsico", "francisco@test.com", "Francisco@1234", ERole.Usuario);

        _usuarioValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<UsuarioDto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

        _usuarioAppServiceMock.Setup(s => s.CriarUsuarioAsync(usuarioDto, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(createdUsuario);

        var result = await _controller.CreateAsync(usuarioDto, CancellationToken.None);

        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        var returnValue = Assert.IsType<UsuarioDto>(createdResult.Value);

        Assert.Equal(createdUsuario.Id, returnValue.Id);
    }

    [Fact]
    public async Task UpdateAsync_IsOwnerOrAdminFalse_ReturnsForbid()
    {
        var usuarioDto = new UsuarioDto(Guid.NewGuid(), "Eduardo", "eduardo@test.com", "Eduardo@1234", ERole.Usuario);

        // Simulando que o usuário logado não é dono nem admin
        var controller = new UsuariosController(_usuarioAppServiceMock.Object, _usuarioValidatorMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                        new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                        new(ClaimTypes.Role, "Usuario")
                }))
                }
            }
        };

        var result = await controller.UpdateAsync(Guid.NewGuid(), usuarioDto, CancellationToken.None);

        Assert.IsType<ForbidResult>(result.Result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsOk()
    {
        var id = Guid.NewGuid();

        _usuarioAppServiceMock.Setup(s => s.DeletarUsuarioAsync(id, It.IsAny<CancellationToken>()))
                           .Returns(Task.CompletedTask);

        var result = await _controller.DeleteAsync(id, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Contains(id.ToString(), okResult.Value!.ToString());
    }

    [Fact]
    public async Task GetPorEmailAsync_UsuarioNaoEncontrado_ReturnsNotFound()
    {
        _usuarioAppServiceMock.Setup(s => s.ObterUsuarioPorEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((UsuarioDto)null!);

        var result = await _controller.GetPorEmailAsync("eduardo@test.com", CancellationToken.None);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetPorEmailAsync_UsuarioEncontrado_ReturnsOk()
    {
        var usuario = new UsuarioDto(Guid.NewGuid(), "Eduardo", "eduardo@test.com", "Eduardo@1234", ERole.Usuario);

        _usuarioAppServiceMock.Setup(s => s.ObterUsuarioPorEmailAsync(usuario.Email!, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(usuario);

        var result = await _controller.GetPorEmailAsync(usuario.Email!, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<UsuarioDto>(okResult.Value);
        Assert.Equal(usuario.Id, returnValue.Id);
    }
}