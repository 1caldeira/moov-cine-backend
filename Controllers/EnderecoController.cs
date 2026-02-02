using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using FilmesAPI.Services;
using FilmesAPI.Data.DTO;
using FluentResults;

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
    public IActionResult AtualizaEnderecoParcial(int id, JsonPatchDocument<UpdateEnderecoDTO> patch) {
        var enderecoParaAtualizar = _enderecoService.RecuperaEnderecoParaAtualizar(id);
        if (enderecoParaAtualizar == null) return NotFound();
        patch.ApplyTo(enderecoParaAtualizar, ModelState);
        if (!TryValidateModel(enderecoParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _enderecoService.AtualizaEndereco(id, enderecoParaAtualizar);
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaEndereco(int id, UpdateEnderecoDTO EnderecoDTO) {
        Result result = _enderecoService.AtualizaEndereco(id, EnderecoDTO);
        if (result.IsFailed) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
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
