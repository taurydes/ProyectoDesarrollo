using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using UCABPagaloTodoWeb.Models;
using UCABPagaloTodoWeb.ResponseHandler;

namespace UCABPagaloTodoWeb.Controllers
{
    public class PrestadorServicioController : Controller
    {
        private readonly ILogger<PrestadorServicioController> _logger;
        private readonly HttpClient client = new HttpClient();

        public PrestadorServicioController()
        {

        }

        /// <summary>
        /// Este método devuelve la vista "RegistrarPrestador.cshtml" que permite al usuario registrar un nuevo proveedor de servicios.
        /// </summary>
        public IActionResult InterfazRegistrar()
        {
            return View("~/Views/PrestadorServicio/RegistrarPrestador.cshtml");

        }

        /// <summary>
        /// Este método recibe un objeto PrestadorModel del formulario de registro de proveedores de servicios. Luego, serializa el objeto a JSON y lo envía a través de una solicitud HTTP POST al endpoint /prestadorservicio/ del controlador de backend. Si la solicitud es exitosa, el método devuelve la vista "Index.cshtml". Si la solicitud falla, se lanza una excepción.
        /// </summary>
        /// <param name="prestador">El objeto PrestadorModel con los datos del proveedor de servicios a registrar.</param>
        [HttpPost]
        public async Task<IActionResult> RegistrarPrestador(PrestadorModel prestador)
        {
            try
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(prestador), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44339/prestadorservicio/", content);

                return View("~/Views/Home/Index.cshtml");

            }
            catch (Exception ex)
            {
                throw ex.InnerException!;
            }
        }

        /// <summary>
        /// Este método envía una solicitud HTTP GET al endpoint 
        /// prestadorservicio del controlador de backend para obtener una lista de todos los proveedores de
        /// servicios registrados. Si la solicitud es exitosa, el método deserializa la respuesta de JSON 
        /// a una lista de objetos PrestadorModel y devuelve la vista "MostrarPrestadores.cshtml"
        /// con la lista de proveedores de servicios. Si la solicitud falla, 
        /// el método maneja el error y devuelve la vista "Index.cshtml".
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GestionPrestadores()
        {
            try
            {
                var response = await client.GetAsync("https://localhost:44339/prestadorservicio");
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var prestadores = JsonConvert.DeserializeObject<List<PrestadorModel>>(responseString);
                    return View("~/Views/PrestadorServicio/MostrarPrestadores.cshtml", prestadores);
                }
                return View("~/Views/Home/Index.cshtml");
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/PrestadorServicio/MostrarPrestadores.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }


        /// <summary>
        /// Este método envía una solicitud HTTP GET al endpoint 
        /// prestadorservicio/{id} del controlador de backend para obtener los detalles de un proveedor
        /// de servicios específico. Si la solicitud es exitosa, el método deserializa 
        /// la respuesta de JSON a un objeto PrestadorModel y devuelve la vista "MostrarPrestador.cshtml"
        /// con los detalles del proveedor de servicios. Si tipo es igual a 2, 
        /// el método devuelve la vista "ActualizarPrestador.cshtml" para que el usuario 
        /// pueda actualizar los detalles del proveedor de servicios. Si la solicitud falla, 
        /// el método maneja el error y devuelve la vista "Index.cshtml".
        /// </summary>
        /// <param name="id">El identificador único del proveedor de servicios.</param>
        /// <param name="tipo">El tipo de consulta. Si es igual a 2, se muestra la vista para actualizar los detalles del proveedor de servicios.</param>
        [HttpGet]
        public async Task<IActionResult> ConsultarPrestadorPorID(Guid id, int tipo)
        {
            try
            {
                var response = await client.GetAsync($"https://localhost:44339/prestadorservicio/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var prestador = JsonConvert.DeserializeObject<PrestadorModel>(responseString);

                    if (tipo == 2) return View("~/Views/PrestadorServicio/ActualizarPrestador.cshtml", prestador);

                    return View("~/Views/PrestadorServicio/MostrarPrestador.cshtml", prestador);
                }

                return View("~/Views/Home/Index.cshtml");
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                return await GestionPrestadores();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }

        /// <summary>
        /// Este método recibe un objeto PrestadorModel del formulario de actualización
        /// de proveedores de servicios. Luego, serializa el objeto a JSON y lo envía a través
        /// de una solicitud HTTP  al endpoint /prestadorservicio/actualizar/{id} del controlador 
        /// de backend para actualizar los detalles de un proveedor de servicios específico. 
        /// Si la solicitud es exitosa, el método redirige al usuario a la vista "MostrarPrestadores.cshtml". 
        /// Si la solicitud falla, el método maneja el error y devuelve la vista "Index.cshtml".
        /// </summary>
        /// <param name="id">El identificador único del proveedor de servicios a actualizar.</param>
        /// <param name="prestador">El objeto PrestadorModel con los nuevos datos del proveedor de servicios.</param>
        public async Task<IActionResult> ActualizarPrestador(Guid id, PrestadorModel prestador)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(prestador), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"https://localhost:44339/prestadorservicio/actualizar/{id}", content);

                return await GestionPrestadores();
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                return await GestionPrestadores();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }

        /// <summary>
        /// Este método envía una solicitud HTTP PUT al endpoint 
        /// /prestadorservicio/estatus/{id} del controlador de backend para eliminar un proveedor
        /// de servicios específico. Si la solicitud es exitosa, 
        /// el método redirige al usuario a la vista "MostrarPrestadores.cshtml". 
        /// Si la solicitud falla, el método maneja el error y devuelve la vista "Index.cshtml".
        /// </summary>
        /// <param name="id">El identificador único del proveedor de servicios a eliminar.</param>
        [HttpPost]
        public async Task<IActionResult> EliminarPrestador(Guid id)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"https://localhost:44339/prestadorservicio/estatus/{id}", content);

                return await GestionPrestadores();

            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                return await GestionPrestadores();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }
    }
}