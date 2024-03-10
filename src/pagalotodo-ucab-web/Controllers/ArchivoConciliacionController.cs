using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Dynamic;
using System.Text;
using UCABPagaloTodoWeb.Models;


namespace UCABPagaloTodoWeb.Controllers
{
    /// <summary>
    /// Controlador encargado de la gestión de archivos de deuda.
    /// </summary>
    public class ArchivoConciliacionController : Controller
    {
        private readonly ILogger<ArchivoConciliacionController> _logger;
        private readonly HttpClient client = new HttpClient();
        private readonly ServicioController _servicio = new ();


        public ArchivoConciliacionController()
        {

        }


        public async Task<IActionResult> InterfazConfigurar(ServicioModel servicio)
        {
            try
            {
                var response = await client.GetAsync($"https://localhost:44339/servicioprestado/{servicio.Id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var servicioResponse = JsonConvert.DeserializeObject<ServicioModel>(responseString);

                    return View("~/Views/ArchivoConciliacion/SeleccionarRangoPago.cshtml", servicioResponse);
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


        public async Task<IActionResult> ConfigurarCampos(PagosServicioPrestadoPorRangoModel servicioRango)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(servicioRango), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"https://localhost:44339/archivoconciliacion/rango", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var servicioResponse = JsonConvert.DeserializeObject<ConfiguracionArchivoConciliacionModel>(responseString);

                    servicioResponse.FechaInicio = servicioRango.FechaInicio;

                    servicioResponse.FechaFin = servicioRango.FechaFin;

                    return View("~/Views/ArchivoConciliacion/ConfiguracionConciliacion.cshtml", servicioResponse);

                    //return View("~/Views/ArchivoConciliacion/SeleccionarRangoPago.cshtml", servicioResponse);
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


        public async Task<IActionResult> RegistrarCierreContableRevision(PagosServicioPrestadoPorRangoModel archivoConfigurado)
        {
            try
            {


                StringContent content = new StringContent(JsonConvert.SerializeObject(archivoConfigurado), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44339/archivoconciliacion/detalle/", content);

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

        public async Task<IActionResult> RegistrarCierreContableAprobado(PagosServicioPrestadoPorRangoModel archivoConfigurado)
        {
            try
            {


                StringContent content = new StringContent(JsonConvert.SerializeObject(archivoConfigurado), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44339/archivoconciliacion/aprobado/", content);

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

        public async Task<IActionResult> ConfigurarCamposAprobado(PagosServicioPrestadoPorRangoModel servicioRango)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(servicioRango), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"https://localhost:44339/archivoconciliacion/rango", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var servicioResponse = JsonConvert.DeserializeObject<ConfiguracionArchivoConciliacionModel>(responseString);

                    servicioResponse.FechaInicio = servicioRango.FechaInicio;

                    servicioResponse.FechaFin = servicioRango.FechaFin;

                    return View("~/Views/ArchivoConciliacion/ConfiguracionConciliacionAprobado.cshtml", servicioResponse);

                    //return View("~/Views/ArchivoConciliacion/SeleccionarRangoPago.cshtml", servicioResponse);
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

        public async Task<IActionResult> InterfazConfigurarAprobado(ServicioModel servicio)
        {
            try
            {
                var response = await client.GetAsync($"https://localhost:44339/servicioprestado/{servicio.Id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var servicioResponse = JsonConvert.DeserializeObject<ServicioModel>(responseString);

                    return View("~/Views/ArchivoConciliacion/SeleccionarRangoPagoAprobado.cshtml", servicioResponse);
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
        /// Muestra la vista para subir un archivo, con los datos del servicio prestado especificado.
        /// </summary>
        /// <param name="Servicio">Datos del servicio asociado al archivo de deuda.</param>
        /// <returns>Vista para subir un archivo.</returns>
        public IActionResult InterfazArchivo(ServicioModel Servicio)
        {

            // Retorna la vista con los datos del servicio
            return View("~/Views/ArchivoConciliacion/SubirArchivoConciliacion.cshtml", Servicio);

        }

        /// <summary>
        /// Procesa el archivo de deuda subido por el usuario, lo convierte en una lista de objetos ArchivoDeudaModel y lo envía a la API.
        /// </summary>
        /// <param name="archivo">Archivo de deuda subido por el usuario.</param>
        /// <param name="servicioId">ID del servicio prestado asociado al archivo de deuda.</param>
        /// <returns>Respuesta HTTP OK si el archivo fue enviado correctamente.</returns>

        public async Task<IActionResult> SubirArchivo(IFormFile archivoXlsx, Guid servicioId)
        {
            // Verifica si el archivo es nulo o tiene longitud cero
            if (archivoXlsx == null || archivoXlsx.Length == 0)
            {
                return BadRequest("No se ha seleccionado ningún archivoXlsx.");
            }

            // Inicia la lectura del archivoXlsx
            using (var package = new ExcelPackage(archivoXlsx.OpenReadStream()))
            {
                // Accede a la primera hoja del archivoXlsx
                var hoja = package.Workbook.Worksheets[0];

                // Lee los encabezados de la hoja y los almacena en una lista
                var encabezados = new List<string>();
                for (int columna = 1; columna <= hoja.Dimension.Columns; columna++)
                {
                    var valorCelda = hoja.Cells[1, columna].Value;
                    if (valorCelda != null)
                    {
                        encabezados.Add(valorCelda.ToString());
                    }
                }

                // Lee los datos de la hoja y los almacena en una lista de objetos dinámicos
                var registros = new List<dynamic>();
                if (hoja.Dimension != null)
                {
                    for (int fila = 2; fila <= hoja.Dimension.Rows; fila++)
                    {
                        var registro = new ExpandoObject() as IDictionary<string, object>;
                        for (int columna = 1; columna <= hoja.Dimension.Columns; columna++)
                        {
                            registro[encabezados[columna - 1]] = hoja.Cells[fila, columna].Value;
                        }
                        registros.Add((ExpandoObject)registro);
                    }
                }
                // Elimina el último registro de la lista de registros
                if (registros.Count > 0)
                {
                    registros.RemoveAt(registros.Count - 1);
                }
                // Crea un objeto ActualizaEstadosPagoRabbitRequest y asigna la lista de registros leídos del archivo a su propiedad Datos
                var model = new ActualizaEstadosPagoRabbitRequest { Datos = registros };

                // Envía el objeto ActualizaEstadosPagoRabbitRequest a la API utilizando HttpClient o un método similar
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"https://localhost:44339/archivoconciliacion/rabbit/", content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Error de conexión con el servidor");
                }
            }
        }

        /// <summary>
        /// actualiza el estado confirmado de la base de datos a través de una petición HTTP PATCH a la URL `https://localhost:44339/archivoconciliacion/estatus/{id}`.
        /// </summary>
        /// <param name="id">El ID del consumidor a eliminar.</param>
        /// <returns>La vista correspondiente a la gestión de consumidores.</returns>
        public async Task<IActionResult> CambiarStatus(Guid id,Guid ServicioPrestadoId)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
                var response = await client.PatchAsync($"https://localhost:44339/archivoconciliacion/estatus/{id}", content);
               
                if (response.IsSuccessStatusCode)
                {
                    return await _servicio.ConsultarServicioPorID(ServicioPrestadoId, 3);

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
