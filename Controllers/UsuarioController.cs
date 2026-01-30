using AutoMapper;
using FilmesAPI.Data.DTOs.Usuario;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[Controller]")]
public class UsuarioController : ControllerBase
{
    private IMapper _mapper;
    private UserManager<Usuario> _userManager;

    public UsuarioController(IMapper mapper, UserManager<Usuario> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> CadastraUsuario(CreateUsuarioDTO dto)
    {
        Usuario usuario = _mapper.Map<Usuario>(dto);

        IdentityResult resultado = await _userManager.CreateAsync(usuario, dto.Password);

        if (resultado.Succeeded) return Ok("Usuário cadastrado com sucesso");

        return BadRequest(resultado.Errors);

    }
}