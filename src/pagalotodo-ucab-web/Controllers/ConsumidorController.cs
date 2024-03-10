using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using UCABPagaloTodoWeb.Models;
using UCABPagaloTodoWeb.ResponseHandler;

namespace UCABPagaloTodoWeb.Controllers
{
    public class ConsumidorController : Controller
    {
        private readonly ILogger<ConsumidorController> _logger;
        private readonly HttpClient client = new HttpClient();

        public ConsumidorController()
        {

        }

        /// <summary>
        /// Devuelve la vista para registrar un nuevo consumidor.
        /// </summary>
        /// <returns>La vista correspondiente a la interfaz de registro de consumidor.</returns>
        public IActionResult InterfazRegistrar()
        {

            return View("~/Views/Consumidor/RegistrarConsumidor.cshtml");

        }

        /// <summary>
        /// Registra un nuevo consumidor en la base de datos a través de una petición HTTP POST a la URL `https://localhost:44339/consumidor/`.
        /// </summary>
        /// <param name="consumidor">Los datos del consumidor a registrar.</param>
        /// <returns>La vista correspondiente a la página principal si el registro es exitoso, o la vista de registro con un mensaje de error en caso contrario.</returns>
        public async Task<IActionResult> RegistrarConsumidor(ConsumidorModel consumidor)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(consumidor), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44339/consumidor/", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var resultId = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    TempData["SuccessMessage"] = "El registro se ha realizado correctamente.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        var errorResponse = JsonConvert.DeserializeObject<ErrorResponseServiceModel>(errorContent);
                        ViewBag.ErrorMessage = string.Join(", ", errorResponse.ErrorMessages);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde.";
                    }

                    // Aquí se devuelve la vista InterfazRegistrar con los datos del consumidor y el mensaje de error.
                    return View("~/Views/Consumidor/RegistrarConsumidor.cshtml");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde.";

                // Aquí se devuelve la vista InterfazRegistrar con los datos del consumidor y el mensaje de error.
                return View("~/Views/Consumidor/RegistrarConsumidor.cshtml", consumidor);
            }
        }

        /// <summary>
        /// Elimina un consumidor de la base de datos a través de una petición HTTP PUT a la URL `https://localhost:44339/consumidor/estatus/{id}`.
        /// </summary>
        /// <param name="id">El ID del consumidor a eliminar.</param>
        /// <returns>La vista correspondiente a la gestión de consumidores.</returns>
        [HttpPut]
        public async Task<IActionResult> EliminarConsumidor(Guid id)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"https://localhost:44339/consumidor/estatus/{id}", content);

                return await GestionConsumidores();

            }
            catch (Exception ex)
            {
                throw ex.InnerException!;
            }
        }

        /// <summary>
        /// Actualiza los datos de un consumidor en la base de datos a través de una petición HTTP PUT a la URL `https://localhost:44339/consumidor/actualizar/{id}`.
        /// </summary>
        /// <param name="id">El ID del consumidor a actualizar.</param>
        /// <param name="consumidor">Los datos actualizados del consumidor.</param>
        /// <returns>La vista correspondiente a la gestión de consumidores.</returns>
        public async Task<IActionResult> ActualizarConsumidor(Guid id, ConsumidorModel consumidor)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(consumidor), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"https://localhost:44339/consumidor/actualizar/{id}", content);

                return await GestionConsumidores();
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                return await GestionConsumidores();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }


        /// <summary>
        /// Devuelve la vista correspondiente a la gestión de consumidores, obteniendo la lista de consumidores registrados en la base de datos a través de una petición HTTP GET a la URL `https://localhost:44339/consumidor`.
        /// </summary>
        /// <returns>La vista correspondiente a la gestión de consumidores.</returns>
        [HttpGet]
        public async Task<IActionResult> GestionConsumidores()
        {
            try
            {
                var response = await client.GetAsync("https://localhost:44339/consumidor");
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var consumidores = JsonConvert.DeserializeObject<List<ConsumidorModel>>(responseString);
                    return View("~/Views/Consumidor/MostrarConsumidores.cshtml", consumidores);
                }
                return View(new List<ConsumidorModel>());
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Consumidor/MostrarConsumidores.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }


        /// <summary>
        /// Obtiene los datos de un consumidor a través de una petición HTTP GET a la URL `https://localhost:44339/consumidor/{id}` y devuelve la vista correspondiente para mostrar los detalles del consumidor.
        /// </summary>
        /// <param name="id">El ID del consumidor a consultar.</param>
        /// <param name="tipo">El tipo de vista que se debe mostrar (1 para la vista de detalles y 2 para la vista de actualización).</param>
        /// <returns>La vista correspondiente para mostrar los detalles del consumidor.</returns>
        [HttpGet]
        public async Task<IActionResult> ConsultarConsumidorPorID(Guid id, int tipo)
        {
            try
            {
                var response = await client.GetAsync($"https://localhost:44339/consumidor/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var consumidor = JsonConvert.DeserializeObject<ConsumidorModel>(responseString);

                    if (tipo == 2) return View("~/Views/Consumidor/ActualizarConsumidor.cshtml", consumidor);

                    return View("~/Views/Consumidor/MostrarConsumidor.cshtml", consumidor);
                }

                return View("~/Views/Home/Index.cshtml");
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                return await GestionConsumidores();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }

        /// <summary>
        /// Obtiene los datos de un consumidor a través de una petición HTTP GET a la URL `https://localhost:44339/consumidor/cedula/{cedula}`.
        /// </summary>
        /// <param name="cedula">La cédula del consumidor a consultar.</param>
        /// <returns>Los datos del consumidor si se encuentra registrado, o null en caso contrario.</returns>
        [HttpGet]
        public async Task<ConsumidorModel> ConsultarPorCedula(int cedula)
        {
            try
            {
                var response = await client.GetAsync($"https://localhost:44339/consumidor/cedula/{cedula}");
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var consumidor = JsonConvert.DeserializeObject<ConsumidorModel>(responseString);

                    if (consumidor != null)
                    {
                        return consumidor;
                    }
                }

                return null; // Retorna null en caso de que no se encuentre el consumidor
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde. {ex}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error. Por favor, inténtelo de nuevo más tarde. {ex}");
                return null;
            }
        }

    }
}