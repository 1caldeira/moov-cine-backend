using FilmesAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs;

public class ReadSessaoDTO
{
    
    public int Id { get; set; }
    public ReadFilmeDTO Filme { get; set; }
    public string Horario { get; set; }

}
