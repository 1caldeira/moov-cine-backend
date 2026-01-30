using FilmesAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs.Sessao;

public class CreateSessaoDTO
{
    [Required]
    public int FilmeId { get; set; }
    [Required]
    public int CinemaId { get; set; }

    [Required(ErrorMessage = "O campo horário é obrigatório")]
    public DateTime Horario { get; set; }

    [Required(ErrorMessage = "O campo sala é obrigatorio")]
    public int Sala { get; set; }

}
