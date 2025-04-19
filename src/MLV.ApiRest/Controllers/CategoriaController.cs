using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLV.ApiRest.Api;
using MLV.Business.Commands;
using MLV.Business.Data.Repository.Interfaces;
using MLV.Business.Models;
using MLV.Business.Services.Interfaces;

namespace MLV.ApiRest.Controllers;

[Authorize]
[Route("api/categorias")]
public class CategoriaController(ICategoriaRepository categoriaRepository,
                      ICategoriaService categoriaService) : MainController
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
    public async Task<ActionResult> Criar(CategoriaRequest request)
    {
        var result = await categoriaService.Adicionar(request);

        if (!result.IsValid)
            return CustomResponse(result);

        return CreatedAtAction(nameof(ObterPorId), new { id = request.Id }, null);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    public async Task<ActionResult> Alterar(Guid id, CategoriaRequest request)
    {
        var categoria = await categoriaRepository.ObterPorId(id);

        if (categoria is null)
            return NotFound();

        request.Id = id;
        var result = await categoriaService.Alterar(request);

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

        var result = await categoriaService.Remover(id);

        if (!result.IsValid)
            return CustomResponse(result);

        return NoContent();
    }
}
