using FilmesAPI.Data.DTOs.Sessao;
using FilmesAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs.Filme;

public class ReadFilmeDTO
{
    public string Titulo { get; set; }
    public string Genero { get; set; }
    public int Duracao { get; set; }
    public DateTime HoraDaConsulta { get; set; } = DateTime.Now;
    public ICollection<ReadSessaoSimpleDTO> Sessoes  { get; set; }
}
