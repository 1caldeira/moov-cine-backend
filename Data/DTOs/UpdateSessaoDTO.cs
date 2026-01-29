using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs;

public class UpdateSessaoDTO
{
    [Required(ErrorMessage = "O campo filme é obrigatório")]
    public int FilmeId { get; set; }

    [Required(ErrorMessage = "O campo horário é obrigatório")]
    public DateTime Horario { get; set; }
}