
namespace FilmesAPI.Data.DTO;

public class ReadCinemaDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public ReadEnderecoDTO Endereco {  get; set; }
    public ICollection<ReadSessaoDTO> Sessoes { get; set; }
    public int NumeroSalas { get; set; }
}
