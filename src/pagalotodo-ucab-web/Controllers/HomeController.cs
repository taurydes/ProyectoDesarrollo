using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UCABPagaloTodoWeb.Models;

namespace UCABPagaloTodoWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
     

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }
        /// <summary>
        /// Devuelve la vista correspondiente a la página de inicio.
        /// </summary>
        /// <returns>La vista correspondiente a la página de inicio.</returns>
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Team()
        {
            return View();
        }

        /// <summary>
        /// Devuelve la vista correspondiente a la página de contacto.
        /// </summary>
        /// <returns>La vista correspondiente a la página de contacto.</returns>
        public IActionResult Contacto()
        {
            return View();
        }

        

        /// <summary>
        /// Devuelve la vista correspondiente a la página de configuración de opciones de pago.
        /// </summary>
        /// <returns>La vista correspondiente a la página de configuración de opciones de pago.</returns>
        public IActionResult ConfigurarOpcionPago()
        {
            return View("~/Views/Administrador/ConfigurarOpcionPago.cshtml");
        }

        

        /// <summary>
        /// Devuelve la vista correspondiente a la política de privacidad.
        /// </summary>
        /// <returns>La vista correspondiente a la política de privacidad.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Devuelve la vista correspondiente a la página de error.
        /// </summary>
        /// <returns>La vista correspondiente a la página de error.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}