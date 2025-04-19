using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLV.ApiRest.Api;
using MLV.Business.Commands;
using MLV.Business.Data.Repository.Interfaces;
using MLV.Business.Models;
using MLV.Business.Services.Interfaces;

namespace MLV.ApiRest.Controllers;

[Authorize]
[Route("api/vendedores")]
public class VendedorController(IVendedorRepository vendedorRepository,
                      IVendedorService vendedorService) : MainController
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
    public async Task<ActionResult> Alterar(Guid id, VendedorRequest request)
    {
        var vendedor = await vendedorRepository.ObterPorId(id);

        if (vendedor is null)
            return NotFound();

        request.Id = id;
        request.Email = request.Email;
        var result = await vendedorService.Alterar(request);

        if (!result.IsValid)
            return CustomResponse(result);

        return NoContent();
    }
}
