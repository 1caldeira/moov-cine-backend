namespace FilmesAPI.Data.DTO;

public class ReadSessaoDTO
{
    
    public int Id { get; set; }
    public ReadFilmeSimpleDTO Filme { get; set; }
    public string Horario { get; set; }
    public int Sala { get; set; }
}
