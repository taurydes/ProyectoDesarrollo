using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using UCABPagaloTodoWeb.Models;

namespace UCABPagaloTodoWeb.Controllers
{
    /// <summary>
    /// Controlador encargado de la gestión de configuraciones de pago.
    /// </summary>
    public class ConfiguracionPagoController : Controller
    {
        private readonly ILogger<ConfiguracionPagoController> _logger;
        private readonly HttpClient client = new HttpClient();
        private readonly CampoController _campoController = new();
                
        public ConfiguracionPagoController()
        {
          
        }

        /// <summary>
        /// Muestra la vista para registrar una nueva configuración de pago, con los datos del servicio especificado.
        /// </summary>
        /// <param name="servicioId">ID del servicio que se desea asociar a la nueva configuración de pago.</param>
        /// <returns>Vista para registrar una nueva configuración de pago.</returns>
        public async Task<IActionResult> InterfazRegistrar(Guid servicioId)
        {
            try
            { 
               // Realiza una petición GET a la API para obtener los datos del servicio.
              // Si la petición es exitosa, se deserializan los datos y se pasan como modelo a la vista.
              // Si la petición falla, se redirige a la página principal (Index.cshtml) y se muestra un mensaje de error.
                var response = await client.GetAsync($"https://localhost:44339/servicioprestado/{servicioId}");
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var servicio = JsonConvert.DeserializeObject<ServicioModel>(responseString);
                    return View("~/Views/ConfiguracionPago/RegistrarConfiguracionPago.cshtml", servicio);
                }
                return View("~/Views/Home/Index.cshtml");
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

        /// <summary>
        /// Registra una nueva configuración de pago, la asocia al servicio correspondiente y agrega su ID a todos los campos que se crearon en la interfaz.
        /// </summary>
        /// <param name="configuracion">Datos de la nueva configuración de pago.</param>
        /// <returns>Página principal (Index.cshtml).</returns>
        [HttpPost]
        public async Task<IActionResult> RegistrarConfiguracionPago(ConfiguracionPagoModel configuracion)
        {
            try 
            { 

                // Realiza una petición POST a la API con los datos de la configuración de pago.
                // Si la petición es exitosa, se deserializa la respuesta para obtener el GUID generado y se asocia la configuración de pago al servicio correspondiente mediante una petición PATCH.
                // Luego, se recorren todos los campos registrados en la interfaz y se les asigna el ID de la configuración de pago.
                // Finalmente, se llama al método RegistrarCampos del controlador CampoController para registrar los campos en la API.
                // Si alguna de las peticiones falla, se redirige a la página principal (Index.cshtml) y se muestra un mensaje de error.

                StringContent content = new StringContent(JsonConvert.SerializeObject(configuracion), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44339/configuracionpago/", content);

                // Si la respuesta es exitosa (200 OK), se deserializa el cuerpo para obtener el GUID generado
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var resultId = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    var resultIdString = resultId.ToString(); // Convertir a cadena
                    var guid = Guid.Parse(resultIdString); // Convertir a Guid
                    
                    configuracion.ConfiguracionPagoId = guid;

                    StringContent contentConfi = new StringContent(JsonConvert.SerializeObject(configuracion), Encoding.UTF8, "application/json");
                    var responseConfi = await client.PatchAsync("https://localhost:44339/servicioprestado/configuracion/", contentConfi);
                   
                    if (configuracion.Campos != null)
                    { 
                        foreach (var item in configuracion.Campos)
                        {
                        item.ConfiguracionPagoId = guid;
                        }
                        await _campoController.RegistrarCampos(configuracion.Campos);
                    }

                }

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