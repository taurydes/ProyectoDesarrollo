using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using UCABPagaloTodoWeb.Models;
using UCABPagaloTodoWeb.ResponseHandler;

namespace UCABPagaloTodoWeb.Controllers
{
    public class ConciliacionController : Controller
    {
        private readonly ILogger<ConsumidorController> _logger;
        private readonly HttpClient client = new HttpClient();

        public ConciliacionController()
        {
           
        }

        public IActionResult InterfazRegistrar()
        {
            return View("~/Views/Conciliacion/ConfigurarArchivo.cshtml");

        }


        /// <summary>
        /// Acción que muestra la interfaz de campos de pago y verifica si el usuario tiene deudas pendientes.
        /// </summary>
        /// <param name="configuracionConciliacion">La configuración de pago seleccionada por el usuario.</param>
        /// <returns>La vista correspondiente según los resultados obtenidos.</returns>
        public async Task<IActionResult> InterfazCamposArchivoConciliacion(ConfiguracionCamposConciliacionModel configuracionConciliacion)
        {
            try
            {
                var configuracion = await ObtenerConfiguracionPago2(configuracionConciliacion);

                return View("~/Views/Conciliacion/ConfigurarArchivo.cshtml");               

            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }

        
        /// <summary>
        /// Método que hace una consulta al controlador "ConfiguracionPagoController" para obtener la configuración de pago correspondiente.
        /// </summary>
        /// <param name="configuracionPago">La configuración de pago seleccionada por el usuario.</param>
        /// <returns>La configuración de pago correspondiente como objeto "ConfiguracionPagoModel".</returns>
        [HttpGet]
        private async Task<ConfiguracionPagoModel> ObtenerConfiguracionPago2(ConfiguracionCamposConciliacionModel configuracionPago)
        {
            var response = await client.GetAsync($"https://localhost:44339/configuracionpago/{configuracionPago.ConfiguracionPagoId}");
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var configuracion = JsonConvert.DeserializeObject<ConfiguracionPagoModel>(responseString);
                return configuracion;
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarConfiguracionConciliacion(ConfiguracionCamposConciliacionModel configuracionPago, List<string> CamposConciliacion)
        {
            // Resto del código para procesar el pago y guardar la información en la base de datos
            return Ok();
        }

    }
}