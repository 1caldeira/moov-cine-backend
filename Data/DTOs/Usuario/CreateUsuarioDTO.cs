using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs.Usuario;

public class CreateUsuarioDTO
{
    [Required]
    public string Username { get; set; }
    [Required]
    public DateTime DataNascimento { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [Compare("Password")]
    public string RePassword { get; set; }
}
