using System.ComponentModel.DataAnnotations;

namespace MLV.MVC.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Este campo é obrigatório")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Este campo é obrigatório")]
    [DataType(DataType.Password)]
    public string Senha { get; set; }
}
