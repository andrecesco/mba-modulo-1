using System.ComponentModel.DataAnnotations;

namespace MLV.ApiRest.ViewModels;

public class UsuarioRegistro
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está no formato inválido")]
    [StringLength(50, ErrorMessage = "O {0} deve ter no minimo {2} e no máximo {1} caracteres.", MinimumLength = 3)]
    public string UserName { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(30, ErrorMessage = "O {0} deve ter no minimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
    public string Senha { get; set; }

    [Compare("Senha", ErrorMessage = "As senhas não conferem")]
    [StringLength(30, ErrorMessage = "O {0} deve ter no minimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
    public string ConfirmaSenha { get; set; }
}

public class UsuarioLogin
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está no formato inválido")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(30, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string Senha { get; set; }
}

public class UsuarioRespostaLogin
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }
}
