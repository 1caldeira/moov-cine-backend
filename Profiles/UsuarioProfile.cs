
using AutoMapper;
using FilmesAPI.Data.DTOs.Usuario;
using FilmesAPI.Models;

namespace FilmesAPI.Profiles;
public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDTO, Usuario>();
        //CreateMap<UpdateUsuarioDTO, Usuario>();
        //CreateMap<Usuario, UpdateUsuarioDTO>();
        //CreateMap<Usuario, ReadUsuarioDTO>();
    }
}
