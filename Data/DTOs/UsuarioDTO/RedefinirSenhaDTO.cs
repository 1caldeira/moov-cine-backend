using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.DTO;
public class RedefinirSenhaDTO
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Token { get; set; }
    [Required]
    public string NovaSenha { get; set; }
    [Required]
    [Compare("NovaSenha", ErrorMessage = "A senha e a confirmação da senha são diferentes.")]
    public string ReNovaSenha { get; set; }
}