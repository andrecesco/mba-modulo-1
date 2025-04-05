using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using MLV.Business.Commands;
using MLV.Business.Interfaces;
using MLV.Business.Models;
using MLV.Core.Messages;
using System.Runtime.CompilerServices;

namespace MLV.Business.Handlers;

public class VendedorHandler(IVendedorRepository repository, IHttpContextAccessor httpContextAccessor) : CommandHandler, IRequestHandler<VendedorCommand, ValidationResult>, IRequestHandler<VendedorAtualizarCommand, ValidationResult>
{
    public async Task<ValidationResult> Handle(VendedorCommand request, CancellationToken cancellationToken)
    {
        var vendedor = new Vendedor(request.Id, request.NomeFantasia, request.Email, request.RazaoSocial, request.NomeFantasia);

        repository.Adicionar(vendedor);

        return await PersistirDados(repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(VendedorAtualizarCommand request, CancellationToken cancellationToken)
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
