using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLV.Business.Commands;
using MLV.Business.Interfaces;
using MLV.Business.Models;
using MLV.Core.Mediator;
using MLV.Infra.Data.Repository;

namespace MLV.MVC.Controllers;

[Authorize]
public class CategoriasController(ICategoriaRepository categoriaRepository,
                      IMediatorHandler mediatorHandler) : Controller
{
    [AllowAnonymous]
    [HttpGet()]
    public async Task<IActionResult> Index()
    {
        var categorias = await categoriaRepository.ObterTodos();

        return View(categorias);
    }

    [HttpGet()]
    public IActionResult Criar()
    {
        return View();
    }

    [HttpPost()]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(CategoriaCriarCommand command)
    {
        var result = await mediatorHandler.EnviarComando(command);

        if (!result.IsValid)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.ErrorMessage);
            }
            return View(nameof(Criar), command);
        }

        return RedirectToAction(nameof(Index));
    }

    [AllowAnonymous]
    public async Task<IActionResult> Alterar(Guid id)
    {
        var categoria = await categoriaRepository.ObterPorId(id);

        if (categoria is null)
            return NotFound();

        return View(new CategoriaAtualizarCommand() { Id = categoria.Id, Nome = categoria.Nome });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Alterar(Guid id, CategoriaAtualizarCommand command)
    {
        var categoria = await categoriaRepository.ObterPorId(id);

        if (categoria is null)
            return NotFound();

        command.Id = id;
        var result = await mediatorHandler.EnviarComando(command);

        if (!result.IsValid)
        {
            foreach(var erro in result.Errors)
            {
                ModelState.AddModelError("", erro.ErrorMessage);
            }

            return View(nameof(Alterar), command);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Remover(Guid? id)
    {
        if (id is null)
            return NotFound();

        var categoria = await categoriaRepository.ObterPorId(id.Value);

        if (categoria is null)
            return NotFound();

        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remover(Guid id)
    {
        var categoria = await categoriaRepository.ObterPorId(id);
        if (categoria is null)
            return NotFound();

        var command = new CategoriaRemoverCommand(id);

        var result = await mediatorHandler.EnviarComando(command);

        if (!result.IsValid)
        {
            foreach (var erro in result.Errors)
            {
                ModelState.AddModelError("", erro.ErrorMessage);
            }

            return View(nameof(Remover), categoria);
        }

        return RedirectToAction(nameof(Index));
    }
}
