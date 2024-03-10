using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using UCABPagaloTodoWeb.Models;

namespace UCABPagaloTodoWeb.Controllers
{
    public class PagoController : Controller
    {
        private readonly ILogger<PagoController> _logger;
        private readonly HttpClient client = new HttpClient();
        private readonly ConsumidorController _consumidor = new ConsumidorController();
        private readonly CampoPagoController _campoController = new();

        public PagoController(ILogger<PagoController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Envía una solicitud HTTP GET al endpoint `prestadorservicio` del controlador de backend para obtener una lista de prestadores de servicio.
        /// Si la solicitud es exitosa, se deserializa la respuesta y se devuelve la vista correspondiente para que el usuario seleccione uno de los prestadores de servicio.
        /// Si la solicitud falla, se muestra un mensaje de error en la vista de inicio.
        /// </summary>
        public async Task<IActionResult> InterfazRegistrar()
        {
            try
            {
                var response = await client.GetAsync("https://localhost:44339/prestadorservicio");
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var prestador = JsonConvert.DeserializeObject<List<PrestadorModel>>(responseString);
                    return View("~/Views/Pago/SeleccionarServicioPago.cshtml", prestador);
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
        /// Acción que muestra la interfaz de campos de pago y verifica si el usuario tiene deudas pendientes.
        /// </summary>
        /// <param name="configuracionPago">La configuración de pago seleccionada por el usuario.</param>
        /// <returns>La vista correspondiente según los resultados obtenidos.</returns>
        public async Task<IActionResult> InterfazCamposPago(ConfiguracionPagoModel configuracionPago)
        {
            try
            {
                if (configuracionPago.TipoPago)
                {
                    var archivoDeuda = await ConsultarArchivoDeuda(configuracionPago);
                    if (archivoDeuda)
                    {
                        var configuracion = await ObtenerConfiguracionPago(configuracionPago);
                        return View("~/Views/Pago/RealizarPago.cshtml", configuracion);
                    }
                    else
                    {
                        return View("~/Views/Pago/RealizarPagoConfirmacionNegada.cshtml");
                    }
                }
                else
                {
                    var configuracion = await ObtenerConfiguracionPago(configuracionPago);
                    return View("~/Views/Pago/RealizarPago.cshtml", configuracion);
                }

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
        /// Método que hace una consulta al controlador "ArchivoDeudaController" para verificar si el usuario tiene deudas pendientes.
        /// </summary>
        /// <param name="configuracionPago">La configuración de pago seleccionada por el usuario.</param>
        /// <returns>True si el usuario tiene deudas pendientes, false en caso contrario.</returns>
        private async Task<bool> ConsultarArchivoDeuda(ConfiguracionPagoModel configuracionPago)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(configuracionPago), Encoding.UTF8, "application/json");
            var responseTipoPago = await client.PostAsync("https://localhost:44339/archivodeuda/consulta", content);
            var responseString = await responseTipoPago.Content.ReadAsStringAsync();
            var archivoDeuda = JsonConvert.DeserializeObject<bool>(responseString);
            return archivoDeuda;
        }

        /// <summary>
        /// Método que hace una consulta al controlador "ConfiguracionPagoController" para obtener la configuración de pago correspondiente.
        /// </summary>
        /// <param name="configuracionPago">La configuración de pago seleccionada por el usuario.</param>
        /// <returns>La configuración de pago correspondiente como objeto "ConfiguracionPagoModel".</returns>
        [HttpGet]
        private async Task<ConfiguracionPagoModel> ObtenerConfiguracionPago(ConfiguracionPagoModel configuracionPago)
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

        /// <summary>
        /// Envía una solicitud HTTP GET al endpoint `servicioprestado/{id}` del controlador de backend para obtener los detalles de un servicio prestado específico.
        /// Si la solicitud es exitosa, se deserializa la respuesta y se devuelve la vista correspondiente para confirmar el pago del servicio.
        /// Si la solicitud falla, se devuelve la vista correspondiente sin detalles adicionales.
        /// </summary>
        public async Task<IActionResult> ConfirmaPago(PagoModel confirmapago)
        {
            var response = await client.GetAsync($"https://localhost:44339/servicioprestado/{confirmapago.ServicioPrestadoId}");
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var servicio = JsonConvert.DeserializeObject<ServicioModel>(responseString);
                //confirmapago.NombreServicio = servicio.Nombre;
                return View("~/Views/Pago/ConfirmaPago.cshtml", confirmapago);
            }
            return View("~/Views/Pago/ConfirmaPago.cshtml", confirmapago);

        }


        /// <summary>
        /// Envía una solicitud HTTP POST al endpoint `pago/` del controlador de backend para registrar un pago.
        /// Si la solicitud es exitosa, se deserializa la respuesta y se obtiene el ID del pago registrado.
        /// Si el pago incluye campos adicionales, se llama al método `RegistrarCampoPago` del controlador `CampoPagoController` para registrarlos.
        /// Si la solicitud falla, se muestra un mensaje de error en la vista de inicio.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RegistrarPago(PagoModel pago)
        {
            try
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(pago), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44339/pago/", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var resultId = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    var resultIdString = resultId.ToString(); // Convertir a cadena
                    var guid = Guid.Parse(resultIdString); // Convertir a Guid


                    if (pago.CamposPago != null)
                    {
                        foreach (var item in pago.CamposPago)
                        {
                            item.PagoId = guid;
                        }
                        return await _campoController.RegistrarCampoPago(pago.CamposPago);
                    }

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
    }
}