using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using UCABPagaloTodoWeb.Models;

namespace UCABPagaloTodoWeb.Controllers
{
    public class CampoPagoController : Controller
    {
        private readonly ILogger<CampoPagoController> _logger;
        private readonly HttpClient client = new HttpClient();
        private readonly ConsumidorController _consumidor = new ConsumidorController();
                
        public CampoPagoController()
        {
        
        }


        /// <summary>
        /// Registra una lista de campos de pago en la base de datos
        /// </summary>
        /// <param name="campos">La lista de campos de pago a registrar</param>
        /// <returns>La vista correspondiente a la página principal</returns>
        [HttpPost]
        public async Task<IActionResult> RegistrarCampoPago(List<CampoPagoModel> campos)
        {
            try
            {
             
                StringContent content = new StringContent(JsonConvert.SerializeObject(campos), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44339/campopago/", content);

                return View("~/Views/Home/Index.cshtml");

                //return await _consumidor.ConsultarConsumidorPorID(campo.ConsumidorId,1);
               
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                //return await GestionPrestadores();
                return View("~/Views/Home/Index.cshtml");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }
    }
}