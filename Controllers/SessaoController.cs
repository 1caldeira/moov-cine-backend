using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.DTOs;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SessaoController : ControllerBase
{
    private AppDbContext _context;
    private IMapper _mapper;
    private const int ToleranciaAtrasoMinutos = 20;

    public SessaoController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AdicionaSessao([FromBody] CreateSessaoDTO SessaoDTO) {
        Sessao Sessao = _mapper.Map<Sessao>(SessaoDTO);
        _context.Sessoes.Add(Sessao);
        _context.SaveChanges();
        ReadSessaoDTO ReadSessaoDTO = _mapper.Map<ReadSessaoDTO>(Sessao);
        return CreatedAtAction(nameof(ObterSessaoPorId), new { Id = Sessao.Id }, ReadSessaoDTO);
    }

    [HttpGet("{id}")]
    public IActionResult ObterSessaoPorId(int id)
    {
        Sessao Sessao = _context.Sessoes.FirstOrDefault(c => c.Id == id)!;
        if (Sessao == null) return NotFound();
        ReadSessaoDTO SessaoDTO = _mapper.Map<ReadSessaoDTO>(Sessao);
        return Ok(SessaoDTO);
    }
    [HttpGet]
    public IEnumerable<ReadSessaoDTO> ObterSessoes([FromQuery] int skip = 0, [FromQuery] int take = 25, [FromQuery] bool apenasDisponiveis = true) {
        
        var query = _context.Sessoes.AsQueryable();

        if (apenasDisponiveis)
        {
            query = query.Where(s => s.Horario.AddMinutes(ToleranciaAtrasoMinutos) > DateTime.Now);
        }

        query = query.OrderBy(s => s.Horario);

        var listaDeSessoes = query.Skip(skip).Take(take).ToList();

        return _mapper.Map<List<ReadSessaoDTO>>(listaDeSessoes);
    }
    

    [HttpPatch("{id}")]
    public IActionResult AtualizaSessaoParcial(int id, JsonPatchDocument<UpdateSessaoDTO> patch) {
        Sessao Sessao = _context.Sessoes.FirstOrDefault(c => c.Id == id)!;
        if (Sessao == null) return NotFound();
        UpdateSessaoDTO SessaoDTO = _mapper.Map<UpdateSessaoDTO>(Sessao);
        patch.ApplyTo(SessaoDTO, ModelState);
        if (!TryValidateModel(SessaoDTO)) {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(SessaoDTO, Sessao);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaSessao(int id, UpdateSessaoDTO SessaoDTO) {
        Sessao Sessao = _context.Sessoes.FirstOrDefault(c => c.Id == id)!;
        if (Sessao == null) return NotFound();
        _mapper.Map(SessaoDTO, Sessao);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaSessao(int id) {
        Sessao Sessao = _context.Sessoes.FirstOrDefault(c => c.Id == id)!;
        if (Sessao == null) return NotFound();
        _context.Remove(Sessao);
        _context.SaveChanges();
        return NoContent();
    }
}
