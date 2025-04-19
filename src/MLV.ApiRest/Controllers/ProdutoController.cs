using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLV.ApiRest.Api;
using MLV.Business.Commands;
using MLV.Business.Data.Repository.Interfaces;
using MLV.Business.Models;
using MLV.Business.Services;

namespace MLV.ApiRest.Controllers;

[Authorize]
[Route("api/produtos")]
public class ProdutoController(IProdutoRepository produtoRepository,
                      ProdutoService produtoService) : MainController
{
    [AllowAnonymous]
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Vendedor>))]
    public async Task<ActionResult> ObterTodos()
    {
        var vendedores = await produtoRepository.ObterTodos();

        return CustomResponse(vendedores);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Categoria))]
    public async Task<ActionResult> ObterPorId(Guid id)
    {
        var produto = await produtoRepository.ObterPorId(id);

        return CustomResponse(produto);
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<ActionResult> Criar(ProdutoRequest request)
    {
        request.Scheme = Request.Scheme;
        request.Host = $"{Request.Host}";

        var result = await produtoService.Adicionar(request);

        if (!result.IsValid)
            return CustomResponse(result);

        return CreatedAtAction(nameof(ObterPorId), new { id = request.Id }, null);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<ActionResult> Alterar(Guid id, ProdutoRequest request)
    {
        var produto = await produtoRepository.ObterPorId(id);

        if (produto is null)
            return NotFound();

        request.Id = id;
        request.Scheme = Request.Scheme;
        request.Host = $"{Request.Host}";
        var result = await produtoService.Alterar(request);

        if (!result.IsValid)
            return CustomResponse(result);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<ActionResult> Remover(Guid id)
    {
        var produto = await produtoRepository.ObterPorId(id);

        if (produto is null)
            return NotFound();

        var result = await produtoService.Remover(id);

        if (!result.IsValid)
            return CustomResponse(result);

        return NoContent();
    }
}
