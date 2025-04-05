using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using MLV.Business.Commands;
using MLV.Business.Interfaces;
using MLV.Business.Models;
using MLV.Core.Messages;
using System.Security.Claims;

namespace MLV.Business.Handlers;


public class CategoriaHandler : CommandHandler, IRequestHandler<CategoriaCriarCommand, ValidationResult>, IRequestHandler<CategoriaAtualizarCommand, ValidationResult>, IRequestHandler<CategoriaRemoverCommand, ValidationResult>
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly string _usuarioLogado;
    private readonly Guid _userId;

    public CategoriaHandler(ICategoriaRepository categoriaRepository, IProdutoRepository produtoRepository, IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null || httpContext.User.Identity == null || !httpContext.User.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("Usuário não está autenticado.");
        }

        _userId = Guid.Parse(httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        _usuarioLogado = httpContext.User.Identity.Name;
        _categoriaRepository = categoriaRepository;
        _produtoRepository = produtoRepository;
    }

    public async Task<ValidationResult> Handle(CategoriaCriarCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid()) return request.ValidationResult;

        var categoria = new Categoria(request.Id, request.Nome, _usuarioLogado);

        if (!await ValidarCategoriaUnicaPorNome(categoria.Nome))
        {
            AdicionarErro("Já existe uma categoria com esse nome.");
            return ValidationResult;
        }

        _categoriaRepository.Adicionar(categoria);

        return await PersistirDados(_categoriaRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CategoriaAtualizarCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid()) return request.ValidationResult;

        var categoria = await _categoriaRepository.ObterPorId(request.Id);

        if (categoria is null)
        {
            AdicionarErro("A categoria não foi encontrada.");
            return ValidationResult;
        }

        categoria.AtualizarNome(request.Nome, _usuarioLogado);

        if (!await ValidarCategoriaUnicaPorNome(categoria.Nome, categoria.Id))
        {
            AdicionarErro("Já existe uma categoria com esse nome.");
            return ValidationResult;
        }

        _categoriaRepository.Atualizar(categoria);

        return await PersistirDados(_categoriaRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CategoriaRemoverCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid()) return request.ValidationResult;

        var categoria = await _categoriaRepository.ObterPorId(request.Id);

        if (categoria is null)
        {
            AdicionarErro("O categoria não foi encontrado.");
            return ValidationResult;
        }

        if (!await ValidarSeNaoTemProdutos(request.Id))
        {
            AdicionarErro("Não é possível remover esta categoria, pois existem produtos associados.");
            return ValidationResult;
        }

        _categoriaRepository.Remover(categoria);

        return await PersistirDados(_categoriaRepository.UnitOfWork);
    }

    public async Task<bool> ValidarCategoriaUnicaPorNome(string nome, Guid? id = null)
    {
        var categoria = await _categoriaRepository.ObterPorNome(nome);

        if (categoria is null) 
            return true;

        if (id.HasValue && categoria.Id == id.Value)
            return true;

        return false;
    }

    public async Task<bool> ValidarSeNaoTemProdutos(Guid id)
    {
        var produtos = await _produtoRepository.ObterPorCategoriaId(id);

        return produtos.Count == 0;
    }
}