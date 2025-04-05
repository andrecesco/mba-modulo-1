using MLV.Core.Messages;

namespace MLV.Business.Commands;

public class VendedorCommand : Command
{
    public Guid Id { get; set; }
    public string NomeFantasia { get; set; }
    public string Email { get; set; }
    public string RazaoSocial { get; set; }
}
