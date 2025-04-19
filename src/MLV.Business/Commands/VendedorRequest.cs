using FluentValidation;
using System.Text.Json.Serialization;

namespace MLV.Business.Commands;

public class VendedorRequest : RequestBase
{
    [JsonIgnore]
    public Guid Id { get; set; }
    [JsonIgnore]
    public string Email { get; set; }
    public string NomeFantasia { get; set; }
    public string RazaoSocial { get; set; }

    public override bool IsValid()
    {
        ValidationResult = new VendedorValidation().Validate(this);

        return ValidationResult.IsValid;
    }

    private sealed class VendedorValidation : AbstractValidator<VendedorRequest>
    {
        public VendedorValidation()
        {
            RuleFor(m => m.NomeFantasia)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido.");
        }
    }
}
