using FilmesAPI.Data.DTOs.Filme;
using FilmesAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs.Sessao;

public class ReadSessaoDTO
{
    
    public int Id { get; set; }
    public ReadFilmeSimpleDTO Filme { get; set; }
    public string Horario { get; set; }
    public int Sala { get; set; }
}
