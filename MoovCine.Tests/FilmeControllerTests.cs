using FilmesAPI.Controllers;
using FilmesAPI.Data.DTO;
using FilmesAPI.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace MoovCine.Tests.Controllers;

public class FilmeControllerTests
{
    private readonly Mock<IFilmeService> _filmeServiceMock;
    private readonly Mock<ITmdbService> _tmdbServiceMock;
    private readonly FilmeController _controller;

    public FilmeControllerTests()
    {
        _filmeServiceMock = new Mock<IFilmeService>();
        _tmdbServiceMock = new Mock<ITmdbService>();
        _controller = new FilmeController(_filmeServiceMock.Object, _tmdbServiceMock.Object);

        var claims = new List<Claim> { new Claim("id", "user-admin-123"), new Claim(ClaimTypes.Role, "admin") };
        var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthType")) };
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    [Fact]
    public void AdicionaFilme_RetornaCreatedAtActionResult()
    {
        var dto = new CreateFilmeDTO { Titulo = "Filme" };
        var readDto = new ReadFilmeDTO { Titulo = "Filme" };
        _filmeServiceMock.Setup(s => s.AdicionaFilme(dto)).Returns(readDto);

        var result = _controller.AdicionaFilme(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var resDto = Assert.IsType<ReadFilmeDTO>(createdResult.Value);
        Assert.Equal("Filme", resDto.Titulo);
    }

    [Fact]
    public void ObterFilmes_RetornaOkResult()
    {
        var filtro = new FiltroFilmeDTO();
        _filmeServiceMock.Setup(s => s.ObterFilmes(filtro)).Returns(new List<ReadFilmeDTO>());
        
        var result = _controller.ObterFilmes(filtro);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ObterFilmesPorId_RetornaOkResult()
    {
        _filmeServiceMock.Setup(s => s.ObterFilmesPorId(1, It.IsAny<bool>(), It.IsAny<bool>())).Returns(new ReadFilmeDTO { Titulo = "F" });
        var result = _controller.ObterFilmesPorId(1);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void AtualizaFilme_Existente_RetornaNoContent()
    {
        var dto = new UpdateFilmeDTO();
        _filmeServiceMock.Setup(s => s.AtualizaFilme(1, dto)).Returns(Result.Ok());
        var result = _controller.AtualizaFilme(1, dto);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeletaFilme_Existente_RetornaNoContent()
    {
        _filmeServiceMock.Setup(s => s.DeletaFilme(1, false)).Returns(Result.Ok());
        var result = _controller.DeletaFilme(1, false);
        Assert.IsType<NoContentResult>(result);
    }
}
