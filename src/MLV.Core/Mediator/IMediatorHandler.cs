using FluentValidation.Results;
using MediatR;
using MLV.Core.Messages;

namespace MLV.Core.Mediator;

public interface IMediatorHandler
{
    Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;
}

public class MediatorHandler(IMediator mediator) : IMediatorHandler
{
    public async Task<ValidationResult> EnviarComando<T>(T comando) where T : Command
    {
        return await mediator.Send(comando);
    }
}