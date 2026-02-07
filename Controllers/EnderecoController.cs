using FilmesAPI.Data.DTO;
using FilmesAPI.Services;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EnderecoController : ControllerBase
{
    private EnderecoService _enderecoService;
   
    public EnderecoController(EnderecoService enderecoService)
    {
        _enderecoService = enderecoService;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public IActionResult AdicionaEndereco([FromBody] CreateEnderecoDTO EnderecoDTO) {

        var readDTO = _enderecoService.AdicionaEndereco(EnderecoDTO);
        return CreatedAtAction(nameof(ObterEnderecoPorId), new { Id = readDTO.Id }, readDTO);
    }

    [HttpGet("{id}")]
    public IActionResult ObterEnderecoPorId(int id)
    {
        var readDTO = _enderecoService.ObterEnderecoPorId(id);
        if(readDTO == null) return NotFound();
        return Ok(readDTO);
    }
    [HttpGet]
    public ActionResult ObterEnderecos([FromQuery] int skip = 0, [FromQuery] int take = 25) {
        var enderecos = _enderecoService.ObterEnderecos(skip, take);
        return Ok(enderecos);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult AtualizaEnderecoParcial(int id, JsonPatchDocument<UpdateEnderecoDTO> patch) {
        var enderecoParaAtualizar = _enderecoService.RecuperaEnderecoParaAtualizar(id);
        if (enderecoParaAtualizar == null) return NotFound();
        patch.ApplyTo(enderecoParaAtualizar, ModelState);
        if (!TryValidateModel(enderecoParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        var resultado = _enderecoService.AtualizaEndereco(id, enderecoParaAtualizar);

        if (resultado.IsFailed)
        {
            return BadRequest(resultado.Errors);
        }

        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult AtualizaEndereco(int id, UpdateEnderecoDTO EnderecoDTO) {
        Result result = _enderecoService.AtualizaEndereco(id, EnderecoDTO);
        if (result.IsFailed) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult DeletaEndereco(int id) {
        Result result = _enderecoService.DeletaEndereco(id);
        if (result.IsFailed) {
            
            if (result.Errors.Any(erro => erro.Message == EnderecoService.ErroNaoEncontrado))
            {
                return NotFound(result.Errors);
            }

            return BadRequest(result.Errors);
        }
        return NoContent();
    }
}
