using FluentValidation.Results;
using MLV.Business.Commands;

namespace MLV.Business.Services.Interfaces;

public interface IVendedorService
{
    Task<ValidationResult> Adicionar(VendedorRequest request);
    Task<ValidationResult> Alterar(VendedorRequest request);
}
