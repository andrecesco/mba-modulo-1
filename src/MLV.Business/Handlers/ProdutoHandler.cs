using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using MLV.Business.Commands;
using MLV.Business.Interfaces;
using MLV.Business.Models;
using MLV.Core.Messages;
using System.Security.Claims;

namespace MLV.Business.Handlers;

public class ProdutoHandler : CommandHandler, IRequestHandler<ProdutoCriarCommand, ValidationResult>, IRequestHandler<ProdutoAtualizarCommand, ValidationResult>, IRequestHandler<ProdutoRemoverCommand, ValidationResult>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly string _usuarioLogado;
    private readonly Guid _userId;
    private readonly string _diretorioBase = "Imagens";

    public ProdutoHandler(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null || httpContext.User.Identity == null || !httpContext.User.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("Usuário não está autenticado.");
        }

        _userId = Guid.Parse(httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        _usuarioLogado = httpContext.User.Identity.Name;
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<ValidationResult> Handle(ProdutoCriarCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid()) return request.ValidationResult;

        var imagemNome = await CarregarImagem(request.Scheme, request.Host, request.Imagem);

        if(string.IsNullOrWhiteSpace(imagemNome))
            return ValidationResult;

        var produto = new Produto(request.Id, request.CategoriaId, _userId, request.Nome, request.Descricao, request.Valor, request.Estoque, imagemNome, _usuarioLogado);

        if(!await ValidarCategoriaExiste(produto.CategoriaId))
        {
            AdicionarErro("Categoria não foi encontrada.");
            return ValidationResult;
        }

        _produtoRepository.Adicionar(produto);

        return await PersistirDados(_produtoRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ProdutoAtualizarCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid()) return request.ValidationResult;

        var produto = await _produtoRepository.ObterPorId(request.Id);

        if(produto is null)
        {
            AdicionarErro("O produto não foi encontrado.");
            return ValidationResult;
        }

        produto.AtualizarProduto(request.CategoriaId, request.Nome, request.Descricao, request.Valor, request.Estoque, _usuarioLogado);

        if (!await ValidarCategoriaExiste(produto.CategoriaId))
        {
            AdicionarErro("A categoria não foi encontrada.");
            return ValidationResult;
        }

        if(!await ValidarProdutoPertenceAoVendedor(produto.Id))
        {
            AdicionarErro("Este produto não pertence ao seu catálogo");
            return ValidationResult;
        }

        _produtoRepository.Atualizar(produto);

        return await PersistirDados(_produtoRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ProdutoRemoverCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid()) return request.ValidationResult;

        var produto = await _produtoRepository.ObterPorId(request.Id);

        if (produto is null)
        {
            AdicionarErro("O produto não foi encontrado.");
            return ValidationResult;
        }

        _produtoRepository.Remover(produto);

        return await PersistirDados(_produtoRepository.UnitOfWork);
    }

    private async Task<bool> ValidarCategoriaExiste(Guid categoriaId)
    {
        var categoria = await _categoriaRepository.ObterPorId(categoriaId);

        return categoria is not null;
    }

    private async Task<bool> ValidarProdutoPertenceAoVendedor(Guid id)
    {
        var produto = await _produtoRepository.ObterPorVendedorId(_userId);

        return produto is not null && produto.FirstOrDefault(p => p.Id == id) is not null;
    }

    private async Task<string> CarregarImagem(string scheme, string host, IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
        {
            AdicionarErro("Nenhum arquivo foi enviado.");
            return string.Empty;
        }

        string extensao = Path.GetExtension(arquivo.FileName);
        string[] extensoesPermitidas = { ".jpg", ".jpeg", ".png", ".gif" };
        if (!Array.Exists(extensoesPermitidas, e => e.Equals(extensao, StringComparison.OrdinalIgnoreCase)))
        {
            AdicionarErro($"Extensão de arquivo não permitida: {extensao}");
            return string.Empty;
        }

        string nomeArquivo = $"{Guid.NewGuid()}{extensao}";

        string caminhoCompleto = Path.Combine(_diretorioBase, nomeArquivo);

        using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        string urlImagem = $"{scheme}://{host}/{_diretorioBase}/{nomeArquivo}";

        return nomeArquivo;
    }
}