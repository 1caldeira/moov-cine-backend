using FilmesAPI.Models;

namespace FilmesAPI.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(Usuario usuario, string role);
}
