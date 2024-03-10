using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UCABPagaloTodoWeb.Models;

namespace UCABPagaloTodoWeb.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly ILogger<AdministradorController> _logger;

        public AdministradorController(ILogger<AdministradorController> logger)
        {
            _logger = logger;
        }

        public IActionResult ConfigurarOpcionPago()
        {
            return View("~/Views/Administrador/ConfigurarOpcionPago.cshtml");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}