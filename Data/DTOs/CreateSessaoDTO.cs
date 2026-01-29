using FilmesAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs;

public class CreateSessaoDTO
{
    [Required]
    public int FilmeId { get; set; }
    [Required]
    public int CinemaId { get; set; }

    [Required(ErrorMessage = "O campo horário é obrigatório")]
    public DateTime Horario { get; set; }

}
