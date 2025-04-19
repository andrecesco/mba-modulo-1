using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using MLV.Business.Commands;
using MLV.Business.Data.Repository.Interfaces;
using MLV.Business.Models;
using MLV.Business.Services.Interfaces;

namespace MLV.Business.Services;

public class VendedorService(IVendedorRepository repository, IHttpContextAccessor httpContextAccessor) : ServiceHandler, IVendedorService
{
    public async Task<ValidationResult> Adicionar(VendedorRequest request)
    {
        var vendedor = new Vendedor(request.Id, request.NomeFantasia, request.Email, request.RazaoSocial, request.NomeFantasia);

        repository.Adicionar(vendedor);

        return await PersistirDados(repository.UnitOfWork);
    }

    public async Task<ValidationResult> Alterar(VendedorRequest request)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null || httpContext.User.Identity == null || !httpContext.User.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("Usuário não está autenticado.");
        }

        var usuarioLogado = httpContextAccessor.HttpContext.User.Identity.Name;

        if (!request.IsValid()) return request.ValidationResult;

        var vendedor = await repository.ObterPorId(request.Id);

        vendedor.AtualizarDados(request.NomeFantasia, request.RazaoSocial, usuarioLogado);

        repository.Atualizar(vendedor);

        return await PersistirDados(repository.UnitOfWork);
    }
}
