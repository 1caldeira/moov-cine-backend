using FilmesAPI.Data.DTO;
using FluentResults;

namespace FilmesAPI.Services.Interfaces;

public interface IFilmeService
{
    ReadFilmeDTO AdicionaFilme(CreateFilmeDTO filmeDTO);
    List<ReadFilmeDTO> ObterFilmes(FiltroFilmeDTO dto);
    ReadFilmeDTO? ObterFilmesPorId(int id, bool isAdmin, bool verSessoesPassadas);
    Result AtualizaFilme(int id, UpdateFilmeDTO filmeDTO);
    UpdateFilmeDTO? RecuperaFilmeParaAtualizar(int id);
    Result DeletaFilme(int id, bool force = false);
}
