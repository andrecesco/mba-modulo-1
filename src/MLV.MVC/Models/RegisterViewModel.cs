using System.ComponentModel.DataAnnotations;

namespace MLV.MVC.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O {0} deve ter no minimo {2} e no máximo {1} caracteres.", MinimumLength = 3)]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está no formato inválido")]
    [StringLength(100, ErrorMessage = "O {0} deve ter no minimo {2} e no máximo {1} caracteres.", MinimumLength = 3)]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DataType(DataType.Password)]
    [StringLength(12, ErrorMessage = "O {0} deve ter no minimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
    public string Senha { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DataType(DataType.Password)]
    [StringLength(12, ErrorMessage = "O {0} deve ter no minimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
    public string ConfirmaSenha { get; set; }
}
