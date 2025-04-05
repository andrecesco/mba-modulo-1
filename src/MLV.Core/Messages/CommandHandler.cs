using FluentValidation.Results;
using MLV.Core.Repository;

namespace MLV.Core.Messages;

public class CommandHandler
{
    private const string MensagemErroSistemico = "Houve um erro sistémico. Por favor, contate o suporte";

    protected ValidationResult ExceptionResult => new ValidationResult(new ValidationFailure[] {
        new ValidationFailure(string.Empty, MensagemErroSistemico)
    });

    protected ValidationResult ValidationResult;

    protected CommandHandler()
    {
        ValidationResult = new ValidationResult();
    }

    protected void AdicionarErro(string mensagem)
    {
        ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
    }

    protected void AdicionarErros(IEnumerable<ValidationFailure> errosDeValidacao)
    {
        ValidationResult.Errors.AddRange(errosDeValidacao);
    }


    protected async Task<ValidationResult> PersistirDados(IUnitOfWork uow)
    {
        if (!await uow.Commit()) AdicionarErro("Houve um erro ao persistir os dados");

        return ValidationResult;
    }
}