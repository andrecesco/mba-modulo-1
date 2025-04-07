using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MLV.Business.Commands;
using MLV.Core.Mediator;
using MLV.MVC.Models;

namespace MLV.MVC.Controllers;

public class AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,
                      IMediatorHandler mediatorHandler) : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet()]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost()]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(
                model.Email, model.Senha, true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Tentativa de login inválida.");
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, model.Senha);

            if (result.Succeeded)
            {
                var resultVendedor = await mediatorHandler.EnviarComando(new VendedorCommand
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    NomeFantasia = model.Nome
                });

                if (!resultVendedor.IsValid)
                {
                    foreach (var item in resultVendedor.Errors)
                    {
                        ModelState.AddModelError("", $"{item.ErrorMessage}");
                    }
                    return View(model);
                }

                var resultAuth = await signInManager.PasswordSignInAsync(
               model.Email, model.Senha, true, lockoutOnFailure: false);

                if (resultAuth.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        return View(model);
    }
}
