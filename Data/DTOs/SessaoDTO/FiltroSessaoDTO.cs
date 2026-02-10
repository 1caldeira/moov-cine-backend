namespace FilmesAPI.Data.DTO;

public class FiltroSessaoDTO
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 20;

    public int? CinemaId { get; set; }
    public int? FilmeId { get; set; }

    public string? NomeFilme { get; set; }

    public bool SomenteDisponiveis { get; set; } = true;
}