using FilmesAPI.Controllers;
using FilmesAPI.Data.DTO;
using FilmesAPI.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MoovCine.Tests.Controllers;

public class CinemaControllerTests
{
    private readonly Mock<ICinemaService> _cinemaServiceMock;
    private readonly CinemaController _controller;

    public CinemaControllerTests()
    {
        _cinemaServiceMock = new Mock<ICinemaService>();
        _controller = new CinemaController(_cinemaServiceMock.Object);
    }

    [Fact]
    public void AdicionaCinema_RetornaCreatedAtActionResult()
    {
        var dto = new CreateCinemaDTO { Nome = "Teste" };
        var readDto = new ReadCinemaDTO { Id = 1, Nome = "Teste" };
        _cinemaServiceMock.Setup(s => s.AdicionaCinema(dto)).Returns(readDto);

        var result = _controller.AdicionaCinema(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var resDto = Assert.IsType<ReadCinemaDTO>(createdResult.Value);
        Assert.Equal(1, resDto.Id);
    }

    [Fact]
    public void ObterCinemaPorId_Existente_RetornaOkResult()
    {
        _cinemaServiceMock.Setup(s => s.ObterCinemaPorId(1)).Returns(new ReadCinemaDTO { Nome = "Test" });
        
        var result = _controller.ObterCinemaPorId(1);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        var resDto = Assert.IsType<ReadCinemaDTO>(okResult.Value);
        Assert.Equal("Test", resDto.Nome);
    }

    [Fact]
    public void ObterCinemaPorId_NaoExistente_RetornaNotFound()
    {
        _cinemaServiceMock.Setup(s => s.ObterCinemaPorId(99)).Returns((ReadCinemaDTO?)null);
        var result = _controller.ObterCinemaPorId(99);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void ObterCinemas_RetornaOkResult()
    {
        _cinemaServiceMock.Setup(s => s.ObterCinemas(0, 25, null)).Returns(new List<ReadCinemaDTO>());
        var result = _controller.ObterCinemas();
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void AtualizaCinema_Existente_RetornaNoContent()
    {
        var dto = new UpdateCinemaDTO { Nome = "Teste" };
        _cinemaServiceMock.Setup(s => s.AtualizaCinema(1, dto)).Returns(Result.Ok());
        var result = _controller.AtualizaCinema(1, dto);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void AtualizaCinema_NaoExistente_RetornaNotFoundResult()
    {
        var dto = new UpdateCinemaDTO { Nome = "Teste" };
        _cinemaServiceMock.Setup(s => s.AtualizaCinema(99, dto)).Returns(Result.Fail(FilmesAPI.Services.CinemaService.ErroNaoEncontrado));
        var result = _controller.AtualizaCinema(99, dto);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void DeletaCinema_Existente_RetornaNoContent()
    {
        _cinemaServiceMock.Setup(s => s.DeletaCinema(1)).Returns(Result.Ok());
        var result = _controller.DeletaCinema(1);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeletaCinema_NaoExistente_RetornaNotFoundObjectResult()
    {
        _cinemaServiceMock.Setup(s => s.DeletaCinema(99)).Returns(Result.Fail(FilmesAPI.Services.CinemaService.ErroNaoEncontrado));
        var result = _controller.DeletaCinema(99);
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
