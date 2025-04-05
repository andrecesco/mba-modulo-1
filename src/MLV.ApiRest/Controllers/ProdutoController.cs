using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLV.Business.Commands;
using MLV.Business.Interfaces;
using MLV.Business.Models;
using MLV.Core.Mediator;
using MLV.Infra.Data.Repository;

namespace MLV.ApiRest.Controllers;

[Authorize]
[Route("api/produtos")]
public class ProdutoController(IProdutoRepository produtoRepository,
                      IMediatorHandler mediatorHandler) : MainController
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
    public async Task<ActionResult> Criar(ProdutoCriarCommand command)
    {
        command.Scheme = Request.Scheme;
        command.Host = $"{Request.Host}";

        var result = await mediatorHandler.EnviarComando(command);

        if (!result.IsValid)
            return CustomResponse(result);

        return CreatedAtAction(nameof(ObterPorId), new { id = command.Id }, null);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<ActionResult> Alterar(Guid id, ProdutoAtualizarCommand command)
    {
        var produto = await produtoRepository.ObterPorId(id);

        if (produto is null)
            return NotFound();

        command.Id = id;
        var result = await mediatorHandler.EnviarComando(command);

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

        var command = new ProdutoRemoverCommand(id);

        var result = await mediatorHandler.EnviarComando(command);

        if (!result.IsValid)
            return CustomResponse(result);

        return NoContent();
    }
}