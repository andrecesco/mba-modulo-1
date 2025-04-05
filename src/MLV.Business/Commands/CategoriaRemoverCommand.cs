using FluentValidation;
using MLV.Core.Messages;
using System.Text.Json.Serialization;

namespace MLV.Business.Commands;

public class CategoriaRemoverCommand(Guid id) : Command
{
    [JsonIgnore]
    public Guid Id { get; set; } = id;

    public override bool IsValid()
    {
        ValidationResult = new CategoriaValidation().Validate(this);

        return ValidationResult.IsValid;
    }

    private sealed class CategoriaValidation : AbstractValidator<CategoriaRemoverCommand>
    {
        public CategoriaValidation()
        {
            RuleFor(m => m.Id)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido");
        }
    }
}