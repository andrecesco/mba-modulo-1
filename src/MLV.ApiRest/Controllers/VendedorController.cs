using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLV.Business.Commands;
using MLV.Business.Interfaces;
using MLV.Business.Models;
using MLV.Core.Api;
using MLV.Core.Mediator;

namespace MLV.ApiRest.Controllers;

[Authorize]
[Route("api/vendedores")]
public class VendedorController(IVendedorRepository vendedorRepository,
                      IMediatorHandler mediatorHandler) : MainController
{
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Vendedor>))]
    public async Task<ActionResult> ObterTodos()
    {
        var vendedores = await vendedorRepository.ObterTodos();

        return CustomResponse(vendedores);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Vendedor))]
    public async Task<ActionResult> ObterPorId(Guid id)
    {
        var vendedor = await vendedorRepository.ObterPorId(id);

        return CustomResponse(vendedor);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Vendedor>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Alterar(Guid id, VendedorAtualizarCommand command)
    {
        var vendedor = await vendedorRepository.ObterPorId(id);

        if(vendedor is null)
            return NotFound();

        command.Id = id;
        var result = await mediatorHandler.EnviarComando(command);

        if (!result.IsValid)
            return CustomResponse(result);

        return NoContent();
    }
}
