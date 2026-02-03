using FilmesAPI.Data.DTO;
using FilmesAPI.Services;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SessaoController : ControllerBase
{
    private SessaoService _sessaoService;
    

    public SessaoController(SessaoService sessaoService)
    {
        _sessaoService = sessaoService;
    }

    [HttpPost]
    public IActionResult AdicionaSessao([FromBody] CreateSessaoDTO createDTO) {
        var result = _sessaoService.AdicionaSessao(createDTO);
        if (result.IsFailed) {
            if (result.Errors.Any(r => r.Message.Equals(CinemaService.ErroNaoEncontrado)
            || r.Message.Equals(FilmeService.ErroNaoEncontrado))){ 
                return NotFound(result.Errors);
            }
            return BadRequest(result.Errors);
        }
        var readDTO = result.Value;
        
        return CreatedAtAction(nameof(ObterSessaoPorId), new { Id = readDTO.Id }, readDTO);
    }

    [HttpGet("{id}")]
    public IActionResult ObterSessaoPorId(int id)
    {
        var sessaoDTO = _sessaoService.ObterSessoesPorId(id);
        if (sessaoDTO == null) return NotFound();
        return Ok(sessaoDTO);
    }
    [HttpGet]
    public IEnumerable<ReadSessaoDTO> ObterSessoes([FromQuery] FiltroSessaoDTO filtro) {
        return _sessaoService.ObterSessoes(filtro);
    }
    

    [HttpPatch("{id}")]
    public IActionResult AtualizaSessaoParcial(int id, JsonPatchDocument<UpdateSessaoDTO> patch) {
        var sessaoParaAtualizar = _sessaoService.RecuperaSessoesParaAtualizar(id);
        if (sessaoParaAtualizar == null) return NotFound();
        patch.ApplyTo(sessaoParaAtualizar, ModelState);
        if (!TryValidateModel(sessaoParaAtualizar)) {
            return ValidationProblem(ModelState);
        }
        Result result = _sessaoService.AtualizaSessoes(id, sessaoParaAtualizar);

        if(result.IsFailed) return BadRequest(result.Errors);
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaSessao(int id, UpdateSessaoDTO sessaoDTO) {
        Result result = _sessaoService.AtualizaSessoes(id, sessaoDTO);
        if (result.IsFailed) {

            if (result.Errors.Any(r => r.Message.Equals(SessaoService.ErroNaoEncontrado)))
            {
                return NotFound();
            }
            return BadRequest(result.Errors);
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaSessao(int id) {
        Result result = _sessaoService.DeletaSessoes(id);
        if (result.IsFailed) {
            if (result.Errors.Any(r => r.Message.Equals(SessaoService.ErroNaoEncontrado)))
            {
                return NotFound(result.Errors);
            }
            return BadRequest(result.Errors);
        }
        return NoContent();
    }
}
