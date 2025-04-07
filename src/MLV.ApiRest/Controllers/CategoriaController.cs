using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLV.Business.Commands;
using MLV.Business.Interfaces;
using MLV.Business.Models;
using MLV.Core.Api;
using MLV.Core.Mediator;
using MLV.Infra.Data.Repository;

namespace MLV.ApiRest.Controllers;

[Authorize]
[Route("api/categorias")]
public class CategoriaController(ICategoriaRepository categoriaRepository,
                      IMediatorHandler mediatorHandler) : MainController
{
    [AllowAnonymous]
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Categoria>))]
    public async Task<ActionResult> ObterTodos()
    {
        var categorias = await categoriaRepository.ObterTodos();

        return CustomResponse(categorias);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Categoria))]
    public async Task<ActionResult> ObterPorId(Guid id)
    {
        var categoria = await categoriaRepository.ObterPorId(id);

        if (categoria is null)
            return NotFound();

        return CustomResponse(categoria);
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<ActionResult> Criar(CategoriaCriarCommand command)
    {
        var result = await mediatorHandler.EnviarComando(command);

        if (!result.IsValid)
            return CustomResponse(result);

        return CreatedAtAction(nameof(ObterPorId), new { id = command.Id }, null);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<ActionResult> Alterar(Guid id, CategoriaAtualizarCommand command)
    {
        var categoria = await categoriaRepository.ObterPorId(id);

        if (categoria is null)
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
        var categoria = await categoriaRepository.ObterPorId(id);

        if (categoria is null)
            return NotFound();

        var command = new CategoriaRemoverCommand(id);

        var result = await mediatorHandler.EnviarComando(command);

        if (!result.IsValid)
            return CustomResponse(result);

        return NoContent();
    }
}
