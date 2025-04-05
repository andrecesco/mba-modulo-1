using FluentValidation;
using MLV.Core.Messages;
using System.Text.Json.Serialization;

namespace MLV.Business.Commands;

public class CategoriaCriarCommand : Command
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Nome { get; set; }

    public CategoriaCriarCommand()
    {
        Id = Guid.NewGuid();
    }

    public override bool IsValid()
    {
        ValidationResult = new CategoriaValidation().Validate(this);

        return ValidationResult.IsValid;
    }

    private sealed class CategoriaValidation : AbstractValidator<CategoriaCriarCommand>
    {
        public CategoriaValidation()
        {
            RuleFor(m => m.Id)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido");
            RuleFor(m => m.Nome)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido")
                .MaximumLength(100)
                .WithMessage("O campo {PropertyName} não pode exceder {MaxLength} caracteres");
        }
    }
}
