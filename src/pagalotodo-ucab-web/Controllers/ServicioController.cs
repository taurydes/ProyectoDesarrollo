using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using UCABPagaloTodoWeb.Models;

namespace UCABPagaloTodoWeb.Controllers
{
    /// <summary>
    /// Controlador para la gestión de servicios prestados.
    /// </summary>
    public class ServicioController : Controller
    {
        private readonly ILogger<ServicioController> _logger;
        private readonly HttpClient client = new HttpClient();
        private readonly PrestadorServicioController _presetadorController = new();
        private readonly ConfiguracionPagoController _configuracionPagoController = new();

        public ServicioController()
        {
        }

        /// <summary>
        /// Interfaz para registrar un servicio prestado.
        /// </summary>
        /// <returns>Una vista con los datos necesarios para registrar un servicio prestado.</returns>
        public async Task<IActionResult> InterfazRegistrar()
        {
            try
            {
                var response = await client.GetAsync("https://localhost:44339/prestadorservicio");
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var prestadores = JsonConvert.DeserializeObject<List<PrestadorModel>>(responseString);
                    return View("~/Views/ServiciosPrestados/RegistrarServicio.cshtml", prestadores);
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


        /// Registra un servicio prestado en la base de datos.
        /// </summary>
        /// <param name="servicio">Los datos del servicio prestado que se desea registrar.</param>
        /// <returns>Una vista para configurar los pagos del servicio prestado.</returns>
        [HttpPost]
        public async Task<IActionResult> RegistrarServicio(ServicioModel servicio)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(servicio), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44339/servicioprestado/", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var resultId = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    var resultIdString = resultId.ToString(); // Convertir a cadena
                    var guid = Guid.Parse(resultIdString); // Convertir a Guid

                    return await _configuracionPagoController.InterfazRegistrar(guid);
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
        /// Consulta un servicio prestado por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del servicio prestado que se desea consultar.</param>
        /// <param name="tipo">El tipo de vista que se desea mostrar.</param>
        /// <returns>La vista correspondiente al tipo de servicio prestado consultado.</returns>
        [HttpGet]
        public async Task<IActionResult> ConsultarServicioPorID(Guid id, int tipo)
        {
            try
            {
                var response = await client.GetAsync($"https://localhost:44339/servicioprestado/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var servicio = JsonConvert.DeserializeObject<ServicioModel>(responseString);
                    if (tipo == 1) return View("~/Views/ServiciosPrestados/MostrarServicio.cshtml", servicio);/*No esta activo aún*/

                    if (tipo == 2) return View("~/Views/ServiciosPrestados/ActualizarServicio.cshtml", servicio);

                    if (tipo == 3) return View("~/Views/ServiciosPrestados/MostrarServicioPagos.cshtml", servicio);

                    if (tipo == 4) return await ServicioConfiguracion(servicio);


                }

                return View("~/Views/Home/Index.cshtml");
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ServicioConfiguracion(ServicioModel servicio)
        {
            try
            {
                var response = await client.GetAsync($"https://localhost:44339/configuracionpago/{servicio.ConfiguracionPagoId}");
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var configuracion = JsonConvert.DeserializeObject<ConfiguracionPagoModel>(responseString);
                    servicio.CamposConfiguracion = configuracion.Campos;
                    return View("~/Views/ServiciosPrestados/MostrarServicioConfiguracionPagos.cshtml", servicio);
                }

                return View("~/Views/Home/Index.cshtml");
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }

        public async Task<IActionResult> ActualizarServicio(Guid id, ServicioModel servicio)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(servicio), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"https://localhost:44339/servicioprestado/actualizar/{id}", content);

                return View("~/Views/Home/Index.cshtml");



            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");


            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> EliminarServicio(PasaDatosModel prestador, Guid id)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"https://localhost:44339/servicioprestado/estatus/{id}", content);

                return await _presetadorController.ConsultarPrestadorPorID(prestador.PrestadorServicioId, 1);
                //return View("~/Views/Home/Index.cshtml");


            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;

                // return await GestionPrestadores();
                return View("~/Views/Home/Index.cshtml");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GestionServicios()
        {
            try
            {
                var response = await client.GetAsync("https://localhost:44339/servicioprestado");
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var servicios = JsonConvert.DeserializeObject<List<ServicioModel>>(responseString);
                    return View("~/Views/ServiciosPrestados/MostrarServicios.cshtml", servicios);
                }
                return View("~/Views/Home/Index.cshtml");
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/ServiciosPrestados/MostrarServicios.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }
    }
}