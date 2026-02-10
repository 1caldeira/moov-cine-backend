using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.DTO;
using FilmesAPI.Models;
using Microsoft.EntityFrameworkCore;
using FluentResults;


namespace FilmesAPI.Services;

public class SessaoService
{
    private IMapper _mapper;
    private AppDbContext _context;

    public SessaoService(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public const string ErroNaoEncontrado = "Sessao não encontrada!";
    public const string ErroSessaoJaPassou = "Só é possivel cancelar uma sessão que ainda não começou";
    public const string ErroHorarioIndisponivel = "O horário escolhido para a nova sessão está indisponível.";
    public const string ErroSessaoNoPassado = "Não é possível criar uma sessão no passado";

    public Result<ReadSessaoDTO> AdicionaSessao(CreateSessaoDTO sessaoDTO)
    {
        var filme = _context.Filmes.FirstOrDefault(f => f.Id == sessaoDTO.FilmeId);
        if (filme == null) return Result.Fail(FilmeService.ErroNaoEncontrado);

        var cinema = _context.Cinemas.FirstOrDefault(c => c.Id == sessaoDTO.CinemaId);
        if (cinema == null) return Result.Fail(CinemaService.ErroNaoEncontrado);

        var horarioFimNovaSessao = sessaoDTO.Horario.AddMinutes(filme.Duracao);

        if (TemConflitoDeHorario(sessaoDTO.CinemaId,sessaoDTO.Sala,sessaoDTO.Horario, filme.Duracao, null))
        {
            return Result.Fail(ErroHorarioIndisponivel);
        }
        if (sessaoDTO.Horario < DateTime.Now) 
        {
            return Result.Fail(ErroSessaoNoPassado);
        }
        Sessao sessao = _mapper.Map<Sessao>(sessaoDTO);
        _context.Sessoes.Add(sessao);
        _context.SaveChanges();
        return Result.Ok(_mapper.Map<ReadSessaoDTO>(sessao));
    }

    public List<ReadSessaoDTO> ObterSessoes(FiltroSessaoDTO filtro)
    {
        var query = _context.Sessoes.AsQueryable();

        if (filtro.CinemaId.HasValue)
        {
            query = query.Where(s => s.CinemaId == filtro.CinemaId);
        }

        if (filtro.FilmeId.HasValue)
        {
            query = query.Where(s => s.FilmeId == filtro.FilmeId);
        }

        if (!string.IsNullOrEmpty(filtro.NomeFilme))
        {
            var termoBusca = filtro.NomeFilme.ToLower();
            query = query.Where(s => s.Filme.Titulo.ToLower().Contains(termoBusca));
        }

        if (filtro.SomenteDisponiveis)
        {
            query = query.Where(s => s.Horario >= DateTime.Now);
        }


        var sessoes = query
            .OrderBy(s => s.Horario)
            .Skip(filtro.Skip)
            .Take(filtro.Take)
            .ToList();

        return _mapper.Map<List<ReadSessaoDTO>>(sessoes);
    }

    public ReadSessaoDTO ObterSessoesPorId(int id)
    {
        var sessao = _context.Sessoes.FirstOrDefault(f => f.Id == id);
        if (sessao == null) return null;
        return _mapper.Map<ReadSessaoDTO>(sessao);
    }

    public Result AtualizaSessoes(int id, UpdateSessaoDTO sessaoDTO)
    {
        var sessao = _context.Sessoes.FirstOrDefault(s => s.Id == id);
        if (sessao == null) return Result.Fail(ErroNaoEncontrado);

        int filmeIdParaValidar = sessaoDTO.FilmeId != 0 ? sessaoDTO.FilmeId : sessao.FilmeId;
        var filmeParaValidar = _context.Filmes.FirstOrDefault(f => f.Id == filmeIdParaValidar);
        if (filmeParaValidar == null) return Result.Fail(FilmeService.ErroNaoEncontrado);

        if (TemConflitoDeHorario(sessao.CinemaId, sessaoDTO.Sala, sessaoDTO.Horario, filmeParaValidar.Duracao, id))
        {
            return Result.Fail(ErroHorarioIndisponivel);
        }

        _mapper.Map(sessaoDTO, sessao);
        _context.SaveChanges();
        return Result.Ok();
    }

    public UpdateSessaoDTO? RecuperaSessoesParaAtualizar(int id)
    {
        var sessao = _context.Sessoes.FirstOrDefault(s => s.Id == id);
        if (sessao == null) return null;
        return _mapper.Map<UpdateSessaoDTO>(sessao);
    }

    public Result DeletaSessoes(int id)
    {
        Sessao sessao = _context.Sessoes.FirstOrDefault(s => s.Id == id)!;
        if (sessao == null) return Result.Fail(ErroNaoEncontrado);

        if (sessao.Horario <= DateTime.Now)
        {
            return Result.Fail(ErroSessaoJaPassou);
        }
        sessao.DataExclusao = DateTime.Now;
        _context.SaveChanges();
        return Result.Ok();
    }

    private bool TemConflitoDeHorario(int cinemaId, int sala, DateTime horarioInicio, int duracaoFilme, int? sessaoIdIgnorar)
    {
        var horarioFim = horarioInicio.AddMinutes(duracaoFilme);

        var query = _context.Sessoes.AsQueryable();

        query = query.Where(s => s.CinemaId == cinemaId
                              && s.Sala == sala
                              && horarioInicio < s.Horario.AddMinutes(s.Filme.Duracao)
                              && horarioFim > s.Horario);

        if (sessaoIdIgnorar.HasValue)
        {
            query = query.Where(s => s.Id != sessaoIdIgnorar.Value);
        }

        return query.Any();
    }
}