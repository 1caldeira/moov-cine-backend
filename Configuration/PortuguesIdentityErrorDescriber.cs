using Microsoft.AspNetCore.Identity;

namespace FilmesAPI.Configuration;
    public class PortuguesIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DefaultError() => new IdentityError { Code = nameof(DefaultError), Description = "Ocorreu um erro desconhecido." };
    public override IdentityError PasswordTooShort(int length) => new IdentityError { Code = nameof(PasswordTooShort), Description = $"A senha deve ter no mínimo {length} caracteres." };
    public override IdentityError PasswordRequiresNonAlphanumeric() => new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "A senha deve conter ao menos um caractere especial (Ex: @, #, !)." };
    public override IdentityError PasswordRequiresDigit() => new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "A senha deve conter ao menos um número ('0'-'9')." };
    public override IdentityError PasswordRequiresLower() => new IdentityError { Code = nameof(PasswordRequiresLower), Description = "A senha deve conter ao menos uma letra minúscula ('a'-'z')." };
    public override IdentityError PasswordRequiresUpper() => new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "A senha deve conter ao menos uma letra maiúscula ('A'-'Z')." };
    public override IdentityError DuplicateUserName(string userName) => new IdentityError { Code = nameof(DuplicateUserName), Description = $"O nome de usuário '{userName}' já está em uso." };
    public override IdentityError DuplicateEmail(string email) => new IdentityError { Code = nameof(DuplicateEmail), Description = $"O e-mail '{email}' já está em uso." };
}

