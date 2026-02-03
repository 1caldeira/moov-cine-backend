namespace FilmesAPI.Data.DTO;

public class FiltroFilmeDTO
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 25;
    public int? CinemaId { get; set; }
    public string? NomeFilme { get; set; }
    public bool ApenasDisponiveis { get; set; } = true;
}