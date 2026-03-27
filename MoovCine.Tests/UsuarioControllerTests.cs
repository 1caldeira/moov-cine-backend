using FilmesAPI.Controllers;
using FilmesAPI.Data.DTO;
using FilmesAPI.DTO;
using FilmesAPI.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace MoovCine.Tests.Controllers;

public class UsuarioControllerTests
{
    private readonly Mock<IUsuarioService> _usuarioServiceMock;
    private readonly Mock<IRabbitMqService> _rabbitMqServiceMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly UsuarioController _controller;

    public UsuarioControllerTests()
    {
        _usuarioServiceMock = new Mock<IUsuarioService>();
        _rabbitMqServiceMock = new Mock<IRabbitMqService>();
        _configurationMock = new Mock<IConfiguration>();

        _configurationMock.Setup(c => c["FrontEndUrl"]).Returns("http://localhost:3000");

        _controller = new UsuarioController(_usuarioServiceMock.Object, _rabbitMqServiceMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task CadastraUsuario_RetornaOkResult()
    {
        var dto = new CreateUsuarioDTO { NomeCompleto = "Teste", Email = "teste@teste.com", Password = "Pass" };
        _usuarioServiceMock.Setup(s => s.Cadastra(dto)).ReturnsAsync(("user123", "token123"));
        _rabbitMqServiceMock.Setup(r => r.PublicarMensagemDeEmailAsync(It.IsAny<MensagemEmailDTO>())).Returns(Task.CompletedTask);

        var result = await _controller.CadastraUsuario(dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Usuário cadastrado com sucesso! Verifique seu e-mail.", okResult.Value);
    }

    [Fact]
    public async Task Login_RetornaOkComToken()
    {
        var dto = new LoginUsuarioDTO { Email = "teste@teste.com", Password = "Pass" };
        _usuarioServiceMock.Setup(s => s.Login(dto)).ReturnsAsync("meu_token_jwt");

        var result = await _controller.Login(dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("meu_token_jwt", okResult.Value);
    }

    [Fact]
    public async Task ConfirmarEmail_Sucesso_RetornaOk()
    {
        var dto = new ConfirmarEmailDTO { UserId = "user123", Token = "token123" };
        _usuarioServiceMock.Setup(s => s.ConfirmaEmail(dto.UserId, dto.Token)).ReturnsAsync(true);

        var result = await _controller.ConfirmarEmail(dto);
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task ConfirmarEmail_Falha_RetornaBadRequest()
    {
        var dto = new ConfirmarEmailDTO { UserId = "user123", Token = "token" };
        _usuarioServiceMock.Setup(s => s.ConfirmaEmail(dto.UserId, dto.Token)).ReturnsAsync(false);

        var result = await _controller.ConfirmarEmail(dto);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task EsqueciMinhaSenha_RetornaOk()
    {
        var dto = new EsqueciMinhaSenhaDTO { Email = "teste@teste.com" };
        _usuarioServiceMock.Setup(s => s.SolicitarRecuperacaoSenha(dto)).Returns(Task.CompletedTask);

        var result = await _controller.EsqueciMinhaSenha(dto);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task RedefinirSenha_Sucesso_RetornaOk()
    {
        var dto = new RedefinirSenhaDTO { Email = "teste@teste.com", NovaSenha = "New", Token = "token" };
        _usuarioServiceMock.Setup(s => s.RedefinirSenhaAsync(dto)).ReturnsAsync(Result.Ok());

        var result = await _controller.RedefinirSenha(dto);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task RedefinirSenha_Falha_RetornaBadRequest()
    {
        var dto = new RedefinirSenhaDTO { Email = "teste@teste.com", NovaSenha = "New", Token = "token" };
        _usuarioServiceMock.Setup(s => s.RedefinirSenhaAsync(dto)).ReturnsAsync(Result.Fail("Erro"));

        var result = await _controller.RedefinirSenha(dto);
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
