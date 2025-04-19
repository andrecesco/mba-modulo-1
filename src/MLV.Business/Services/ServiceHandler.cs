using FluentValidation.Results;
using MLV.Core.Repository;

namespace MLV.Business.Services;

public class ServiceHandler
{
    private const string _mensagemErroSistemico = "Houve um erro sistémico. Por favor, contate o suporte";

    protected ValidationResult ValidationResult;

    protected ServiceHandler()
    {
        ValidationResult = new ValidationResult();
    }

    protected void AdicionarErro(string mensagem)
    {
        ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
    }

    protected async Task<ValidationResult> PersistirDados(IUnitOfWork uow)
    {
        if (!await uow.Commit()) AdicionarErro(_mensagemErroSistemico);

        return ValidationResult;
    }
}
