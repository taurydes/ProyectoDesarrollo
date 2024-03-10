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
    /// <summary>
    /// Controlador encargado de la gestión de archivos de deuda.
    /// </summary>
    public class ArchivoDeudaController : Controller
    {
        private readonly ILogger<ArchivoDeudaController> _logger;
        private readonly HttpClient client = new HttpClient();
        private readonly ConsumidorController _consumidor = new();

        public ArchivoDeudaController()
        {
           
        }

        /// <summary>
        /// Muestra la vista para subir un archivo de deuda, con los datos del servicio prestado especificado.
        /// </summary>
        /// <param name="Servicio">Datos del servicio asociado al archivo de deuda.</param>
        /// <param name="ServicioPrestadoId">ID del servicio prestado asociado al archivo de deuda.</param>
        /// <returns>Vista para subir un archivo de deuda.</returns>
        public IActionResult InterfazRegistrar(ServicioModel Servicio,Guid ServicioPrestadoId)
        {
            // Asigna el ID del servicio prestado al servicio en el modelo
            Servicio.Id = ServicioPrestadoId;

            // Retorna la vista con los datos del servicio
            return View("~/Views/ArchivoDeuda/SubirArchivoDeuda.cshtml", Servicio);

        }

        /// <summary>
        /// Procesa el archivo de deuda subido por el usuario, lo convierte en una lista de objetos ArchivoDeudaModel y lo envía a la API.
        /// </summary>
        /// <param name="archivo">Archivo de deuda subido por el usuario.</param>
        /// <param name="servicioId">ID del servicio prestado asociado al archivo de deuda.</param>
        /// <returns>Respuesta HTTP OK si el archivo fue enviado correctamente.</returns>
        [HttpPost]
        public async Task<IActionResult> SubirArchivo(IFormFile archivo, Guid servicioId)
        {
            // Verifica si el archivo es nulo o tiene longitud cero
            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest("No se ha seleccionado ningún archivo.");
            }

            // Crea una lista para almacenar los objetos ArchivoDeudaModel
            var deudas = new List<ArchivoDeudaModel>();

            // Inicia la lectura del archivo
            using (var reader = new StreamReader(archivo.OpenReadStream()))
            {
                // Leer la cabecera del archivo y validar que cumpla con el estándar esperado
                var cabecera = await reader.ReadLineAsync();
                if (cabecera != "Cedula,Nombre,Apellido,Deuda")
                {
                    return BadRequest("El archivo no cumple el estándar esperado.");
                }

                while (!reader.EndOfStream)
                {
                    // Lee una línea del archivo
                    var linea = await reader.ReadLineAsync();

                    // Si la línea está vacía, sale del bucle
                    if (string.IsNullOrEmpty(linea))
                    {
                        break;
                    }

                    // Divide la línea en campos y crea un objeto ArchivoDeudaModel
                    var campos = linea.Split(',');

                    var deuda = new ArchivoDeudaModel
                    {
                        Cedula = int.Parse(campos[0]),
                        Nombre = campos[1],
                        Apellido = campos[2],
                        Deuda = decimal.Parse(campos[3]),
                        ServicioPrestadoId = servicioId
                    };

                    // Consulta el consumidor asociado a la cédula de la deuda en la API de consumidores
                    var result = await _consumidor.ConsultarPorCedula(deuda.Cedula);
                    if (result != null)
                    {
                        deuda.ConsumidorId = result.Id;
                    }

                    // Agrega el objeto ArchivoDeudaModel a la lista
                    deudas.Add(deuda);
                }
            }

            // Si la lista no está vacía, envía los datos a la API
            if (deudas != null)
            {
                await EnviarArchivo(deudas);
            }

            // Retorna una respuesta HTTP OK con la lista de objetos ArchivoDeudaModel convertida a JSON
            return Ok(JsonConvert.SerializeObject(deudas));
        }


        /// <summary>
        /// Envía la lista de objetos ArchivoDeudaModel a la API para ser registrados.
        /// </summary>
        /// <param name="archivo">Lista de objetos ArchivoDeudaModel.</param>
        /// <returns>Respuesta HTTP OK si la petición serealizó correctamente.</returns>
        public async Task<IActionResult> EnviarArchivo(List<ArchivoDeudaModel> archivo)
        {
            try
            {
                // Convierte la lista de objetos ArchivoDeudaModel a un JSON y lo envía en una petición POST a la API
                StringContent content = new StringContent(JsonConvert.SerializeObject(archivo), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44339/archivodeuda", content);

                // Si la petición es exitosa, retorna una respuesta HTTP OK
                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                // Si la petición falla, retorna una respuesta HTTP BadRequest con el contenido del error
                else
                {
                    return BadRequest(response.Content);
                }
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                // Retorna la vista principal
                return View("~/Views/Home/Index.cshtml");
            }
            catch (Exception ex)
            {
                // Si ocurre cualquier otro error, se muestra un mensaje de error en la vista.
                ViewBag.ErrorMessage = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                // Retorna la vista principal
                return View("~/Views/Home/Index.cshtml");
            }
        }
    }


}
