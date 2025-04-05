using FluentValidation;
using MLV.Core.Messages;
using System.Text.Json.Serialization;

namespace MLV.Business.Commands;

public class ProdutoRemoverCommand(Guid id) : Command
{
    [JsonIgnore]
    public Guid Id { get; set; } = id;

    public override bool IsValid()
    {
        ValidationResult = new ProdutoValidation().Validate(this);

        return ValidationResult.IsValid;
    }

    private sealed class ProdutoValidation : AbstractValidator<ProdutoRemoverCommand>
    {
        public ProdutoValidation()
        {
            RuleFor(m => m.Id)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido");
        }
    }
}