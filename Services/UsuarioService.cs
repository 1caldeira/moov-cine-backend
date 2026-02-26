using AutoMapper;
using FilmesAPI.Data.DTO;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace FilmesAPI.Services;

public class UsuarioService
{
    private IMapper _mapper;
    private UserManager<Usuario> _userManager;
    private SignInManager<Usuario> _signInManager;
    private TokenService _tokenService;
    private RoleManager<IdentityRole> _roleManager;

    public UsuarioService(IMapper mapper, UserManager<Usuario> userManager, 
        SignInManager<Usuario> signInManager, TokenService tokenService, RoleManager<IdentityRole> roleManager)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _roleManager = roleManager;
    }

    public async Task<string> Login(LoginUsuarioDTO dto)
    {
        var resultado = await _signInManager.PasswordSignInAsync(dto.Username,dto.Password,false,false);
        if (!resultado.Succeeded) {
            throw new ApplicationException("Usuario nao autenticado!");
        }
        var usuario = _signInManager.UserManager.Users.FirstOrDefault(
            user => user.NormalizedUserName == dto.Username.ToUpper());

        var roles = await _signInManager.UserManager.GetRolesAsync(usuario);
        var roleDoUsuario = roles.FirstOrDefault() ?? "usuario";

        var token = _tokenService.GenerateToken(usuario, roleDoUsuario);
        return token;

    }

    public async Task Cadastra(CreateUsuarioDTO dto) {
        Usuario usuario = _mapper.Map<Usuario>(dto);

        IdentityResult resultado = await _userManager.CreateAsync(usuario, dto.Password);

        if (!resultado.Succeeded) {
            var mensagensDeErro = resultado.Errors.Select(e => e.Description);
            var erroFormatado = string.Join(" | ", mensagensDeErro);

            throw new ApplicationException(erroFormatado);
        }


    }


}

