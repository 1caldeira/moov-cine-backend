using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Usuario : IdentityUser
{
    public DateTime DataNascimento { get; set; }
    public DateTime? DataExclusao { get; set; }
    public Usuario() : base()
    {
        
    }
}
