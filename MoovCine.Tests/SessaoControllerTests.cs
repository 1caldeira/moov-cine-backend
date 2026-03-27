using FilmesAPI.Controllers;
using FilmesAPI.Data.DTO;
using FilmesAPI.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MoovCine.Tests.Controllers;

public class SessaoControllerTests
{
    private readonly Mock<ISessaoService> _sessaoServiceMock;
    private readonly SessaoController _controller;

    public SessaoControllerTests()
    {
        _sessaoServiceMock = new Mock<ISessaoService>();
        _controller = new SessaoController(_sessaoServiceMock.Object);
    }

    [Fact]
    public void AdicionaSessao_Test_RetornaCreatedAt()
    {
        var dto = new CreateSessaoDTO();
        var readDto = new ReadSessaoDTO { Id = 1 };
        _sessaoServiceMock.Setup(s => s.AdicionaSessao(dto)).Returns(Result.Ok(readDto));

        var result = _controller.AdicionaSessao(dto);
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public void ObterSessoes_RetornaOkResult()
    {
        var filtro = new FiltroSessaoDTO();
        _sessaoServiceMock.Setup(s => s.ObterSessoes(filtro)).Returns(new List<ReadSessaoDTO>());
        
        var result = _controller.ObterSessoes(filtro);
        Assert.IsAssignableFrom<IEnumerable<ReadSessaoDTO>>(result);
    }

    [Fact]
    public void ObterSessoesPorId_RetornaOkResult()
    {
        _sessaoServiceMock.Setup(s => s.ObterSessoesPorId(1)).Returns(new ReadSessaoDTO { Id = 1 });
        var result = _controller.ObterSessaoPorId(1);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void AtualizaSessoes_Existente_RetornaNoContent()
    {
        var dto = new UpdateSessaoDTO();
        _sessaoServiceMock.Setup(s => s.AtualizaSessoes(1, dto)).Returns(Result.Ok());
        var result = _controller.AtualizaSessao(1, dto);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeletaSessoes_Existente_RetornaNoContent()
    {
        _sessaoServiceMock.Setup(s => s.DeletaSessoes(1)).Returns(Result.Ok());
        var result = _controller.DeletaSessao(1);
        Assert.IsType<NoContentResult>(result);
    }
}
