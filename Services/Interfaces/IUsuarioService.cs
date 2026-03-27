using FilmesAPI.Data.DTO;
using FilmesAPI.DTO;
using FluentResults;
using System.Threading.Tasks;

namespace FilmesAPI.Services.Interfaces;

public interface IUsuarioService
{
    Task<string> Login(LoginUsuarioDTO dto);
    Task<(string usuarioId, string token)> Cadastra(CreateUsuarioDTO dto);
    Task<bool> ConfirmaEmail(string userId, string token);
    Task SolicitarRecuperacaoSenha(EsqueciMinhaSenhaDTO dto);
    Task<Result> RedefinirSenhaAsync(RedefinirSenhaDTO dto);
}
