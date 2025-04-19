using FluentValidation.Results;
using MLV.Business.Commands;

namespace MLV.Business.Services.Interfaces;

public interface IProdutoService
{
    Task<ValidationResult> Adicionar(ProdutoRequest request);
    Task<ValidationResult> Alterar(ProdutoRequest request);
    Task<ValidationResult> Remover(Guid id);
}
