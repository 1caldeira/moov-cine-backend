using FilmesAPI.Controllers;
using FilmesAPI.Data.DTO;
using FilmesAPI.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MoovCine.Tests.Controllers;

public class EnderecoControllerTests
{
    private readonly Mock<IEnderecoService> _enderecoServiceMock;
    private readonly EnderecoController _controller;

    public EnderecoControllerTests()
    {
        _enderecoServiceMock = new Mock<IEnderecoService>();
        _controller = new EnderecoController(_enderecoServiceMock.Object);
    }

    [Fact]
    public void AdicionaEndereco_RetornaCreatedAtActionResult()
    {
        var dto = new CreateEnderecoDTO { Logradouro = "Rua Teste" };
        var readDto = new ReadEnderecoDTO { Id = 1, Logradouro = "Rua Teste" };
        _enderecoServiceMock.Setup(s => s.AdicionaEndereco(dto)).Returns(readDto);

        var result = _controller.AdicionaEndereco(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var resDto = Assert.IsType<ReadEnderecoDTO>(createdResult.Value);
        Assert.Equal(1, resDto.Id);
    }

    [Fact]
    public void ObterEnderecoPorId_Existente_RetornaOkResult()
    {
        _enderecoServiceMock.Setup(s => s.ObterEnderecoPorId(1)).Returns(new ReadEnderecoDTO { Logradouro = "Rua" });
        var result = _controller.ObterEnderecoPorId(1);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void ObterEnderecoPorId_NaoExistente_RetornaNotFound()
    {
        _enderecoServiceMock.Setup(s => s.ObterEnderecoPorId(99)).Returns((ReadEnderecoDTO?)null);
        var result = _controller.ObterEnderecoPorId(99);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void ObterEnderecos_RetornaOkResult()
    {
        _enderecoServiceMock.Setup(s => s.ObterEnderecos(0, 25)).Returns(new List<ReadEnderecoDTO>());
        var result = _controller.ObterEnderecos();
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void AtualizaEndereco_Existente_RetornaNoContent()
    {
        var dto = new UpdateEnderecoDTO { Logradouro = "Teste" };
        _enderecoServiceMock.Setup(s => s.AtualizaEndereco(1, dto)).Returns(Result.Ok());
        var result = _controller.AtualizaEndereco(1, dto);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeletaEndereco_Existente_RetornaNoContent()
    {
        _enderecoServiceMock.Setup(s => s.DeletaEndereco(1)).Returns(Result.Ok());
        var result = _controller.DeletaEndereco(1);
        Assert.IsType<NoContentResult>(result);
    }
}
