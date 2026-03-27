using FilmesAPI.Data.DTO;
using FluentResults;
using System.Threading.Tasks;

namespace FilmesAPI.Services.Interfaces;

public interface ISessaoService
{
    Result<ReadSessaoDTO> AdicionaSessao(CreateSessaoDTO sessaoDTO);
    List<ReadSessaoDTO> ObterSessoes(FiltroSessaoDTO filtro);
    ReadSessaoDTO ObterSessoesPorId(int id);
    Result AtualizaSessoes(int id, UpdateSessaoDTO sessaoDTO);
    UpdateSessaoDTO? RecuperaSessoesParaAtualizar(int id);
    Result DeletaSessoes(int id);
    Task<int> GerarSessoesAutomaticamente();
}
