using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs.Endereco;

public class ReadEnderecoDTO
{
    public int Id { get; set; }
    public string Logradouro { get; set; }

    public int Numero { get; set; }
}
