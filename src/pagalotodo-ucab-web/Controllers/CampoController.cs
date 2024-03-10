using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using UCABPagaloTodoWeb.Models;

namespace UCABPagaloTodoWeb.Controllers
{
    public class CampoController : Controller
    {
        private readonly ILogger<CampoController> _logger;
        private readonly HttpClient client = new HttpClient();
        //private readonly ServicioController _servicio = new ServicioController(); // hay que revisar lo de las dependencias
                
        public CampoController()
        {
        
        }

        /// <summary>
        /// Muestra la interfaz de registro de campos para una configuración de pago específica
        /// </summary>
        /// <param name="configuracionPagoId">El identificador de la configuración de pago</param>
        /// <returns>La vista correspondiente a la interfaz de registro de campos</returns>
        public IActionResult InterfazRegistrar(Guid configuracionPagoId )
        {
            ConfiguracionPagoModel configuracionPago = new ConfiguracionPagoModel();
            configuracionPago.ConfiguracionPagoId=configuracionPagoId;
            return View("~/Views/Campo/RegistrarCampo.cshtml", configuracionPago);

        }

        /// <summary>
        /// Registra un campo en la base de datos
        /// </summary>
        /// <param name="campo">El campo a registrar</param>
        /// <returns>La vista correspondiente a la página principal</returns>
        [HttpPost]
        public async Task<IActionResult> RegistrarCampo(CampoModel campo)
        {
            try
            {
             
                StringContent content = new StringContent(JsonConvert.SerializeObject(campo), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44339/campo/campo", content);

                return View("~/Views/Home/Index.cshtml");
                // SE DEBE AGRGAR LA OPCION DE QUE VAYA AL SERVICIO QUE PERTENECE
               
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
        /// Registra varios campos en la base de datos
        /// </summary>
        /// <param name="campos">La lista de campos a registrar</param>
        /// <returns>La vista correspondiente a la página principal</returns>
        [HttpPost]
        public async Task<IActionResult> RegistrarCampos(List<CampoModel> campos)
        {
            try
            {
             
                StringContent content = new StringContent(JsonConvert.SerializeObject(campos), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44339/campo/", content);

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
        /// Consulta un campo por su identificador
        /// </summary>
        /// <param name="id">El identificador del campo a consultar</param>
        /// <param name="ServicioPrestadoId">El identificador del servicio prestado</param>
        /// <returns>La vista correspondiente a la interfaz de actualización de campos</returns>
        [HttpGet]
        public async Task<IActionResult> ConsultarCampoPorID(Guid id,Guid ServicioPrestadoId)
        {
            try
            {
                var response = await client.GetAsync($"https://localhost:44339/campo/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var campo = JsonConvert.DeserializeObject<CampoModel>(responseString);
                    campo.ServicioPrestadoId = ServicioPrestadoId;
                    return View("~/Views/Campo/ActualizarCampo.cshtml", campo);      
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


        /// <summary>
        /// Elimina un campo de la base de datos
        /// </summary>
        /// <param name="id">El identificador del campo a eliminar</param>
        /// <returns>La vista correspondiente a la página principal</returns>
        [HttpDelete]
        public async Task<IActionResult> EliminarCampo(Guid id)
        {
            try
            {
                var response = await client.DeleteAsync($"https://localhost:44339/campo/{id}");
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

        /// <summary>
        /// Actualiza un campo en la base de datos
        /// </summary>
        /// <param name="id">El identificador del campo a actualizar</param>
        /// <param name="campo">Los datos actualizados del campo</param>
        /// <returns>La vista correspondiente a la página principal</returns>
        [HttpPatch]
        public async Task<IActionResult> Actualizar(Guid id, CampoModel campo)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(campo), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"https://localhost:44339/campo/actualizar/{id}", content);

                return View("~/Views/Home/Index.cshtml");

               // return await _servicio.ConsultarServicioPorID(campo.ServicioPrestadoId, 4);
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
    }
}