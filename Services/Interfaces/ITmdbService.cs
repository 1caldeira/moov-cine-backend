using FluentResults;
using System.Threading.Tasks;

namespace FilmesAPI.Services.Interfaces;

public interface ITmdbService
{
    Task<Result> ImportarFilmesNowPlaying();
    Task<Result> ImportarFilmesUpcoming();
}
