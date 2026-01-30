using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs.Endereco;

public class UpdateEnderecoDTO
{

    public string Logradouro { get; set; }

    public int Numero { get; set; }
}
