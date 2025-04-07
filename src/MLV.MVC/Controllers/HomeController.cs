using Microsoft.AspNetCore.Mvc;
using MLV.Business.Interfaces;
using MLV.MVC.Models;
using System.Diagnostics;

namespace MLV.MVC.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IProdutoRepository produtoRepository) : Controller
    {
        public async Task<IActionResult> Index()
        {
            logger.LogInformation("Listando os produtos");
            var produtos = await produtoRepository.ObterTodos();
            return View(produtos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
