using FilmesAPI.Data.DTO;
using FluentResults;
using System.Collections.Generic;

namespace FilmesAPI.Services.Interfaces;

public interface IEnderecoService
{
    ReadEnderecoDTO AdicionaEndereco(CreateEnderecoDTO enderecoDTO);
    ReadEnderecoDTO? ObterEnderecoPorId(int id);
    List<ReadEnderecoDTO> ObterEnderecos(int skip, int take);
    UpdateEnderecoDTO? RecuperaEnderecoParaAtualizar(int id);
    Result AtualizaEndereco(int id, UpdateEnderecoDTO enderecoDto);
    Result DeletaEndereco(int id);
}
