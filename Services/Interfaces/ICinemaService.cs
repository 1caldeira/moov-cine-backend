using FilmesAPI.Data.DTO;
using FluentResults;

namespace FilmesAPI.Services.Interfaces;

public interface ICinemaService
{
    ReadCinemaDTO AdicionaCinema(CreateCinemaDTO cinemaDTO);
    ReadCinemaDTO? ObterCinemaPorId(int id);
    List<ReadCinemaDTO> ObterCinemas(int skip, int take, int? enderecoId);
    UpdateCinemaDTO? RecuperaCinemaParaAtualizar(int id);
    Result AtualizaCinema(int id, UpdateCinemaDTO cinemaDto);
    Result DeletaCinema(int id);
}
