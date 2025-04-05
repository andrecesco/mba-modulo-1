using FluentValidation;
using MLV.Core.Messages;
using System.Text.Json.Serialization;

namespace MLV.Business.Commands;

public class VendedorAtualizarCommand : Command
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string NomeFantasia { get; set; }
    public string RazaoSocial { get; set; }

    public override bool IsValid()
    {
        ValidationResult = new VendedorValidation().Validate(this);

        return ValidationResult.IsValid;
    }

    private sealed class VendedorValidation : AbstractValidator<VendedorAtualizarCommand>
    {
        public VendedorValidation()
        {
            RuleFor(m => m.NomeFantasia)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido.");
        }
    }
}
