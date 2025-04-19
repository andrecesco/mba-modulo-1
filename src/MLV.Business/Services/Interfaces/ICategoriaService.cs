using FluentValidation.Results;
using MLV.Business.Commands;

namespace MLV.Business.Services.Interfaces;

public interface ICategoriaService
{
    Task<ValidationResult> Adicionar(CategoriaRequest request);
    Task<ValidationResult> Alterar(CategoriaRequest request);
    Task<ValidationResult> Remover(Guid id);
}
