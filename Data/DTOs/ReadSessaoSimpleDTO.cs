using FilmesAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs;

public class ReadSessaoSimpleDTO
{
    
    public int Id { get; set; }
    public string Horario { get; set; }
    public int Sala { get; set; }
}
