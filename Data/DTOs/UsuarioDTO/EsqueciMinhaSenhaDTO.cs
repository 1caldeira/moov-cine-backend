using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTO;

public class EsqueciMinhaSenhaDTO
{
    [Required]
    public string Email { get; set; }

}
