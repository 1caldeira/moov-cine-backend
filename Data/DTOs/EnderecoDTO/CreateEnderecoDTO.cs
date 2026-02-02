using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTO;

public class CreateEnderecoDTO
{
    [Required]
    public string Logradouro { get; set; }
    [Required]
    public int Numero { get; set; }
    
}
