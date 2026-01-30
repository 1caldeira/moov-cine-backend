using FilmesAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs.Filme;

public class ReadFilmeSimpleDTO
{
    public string Titulo { get; set; }
    public string Genero { get; set; }
    public int Duracao { get; set; }
}
