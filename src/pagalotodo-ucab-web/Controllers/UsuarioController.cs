using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using UCABPagaloTodoWeb.Models;
using UCABPagaloTodoWeb.ResponseHandler;

namespace UCABPagaloTodoWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly HttpClient _client = new HttpClient();
 
        public UsuarioController()
        {

        }
        /// <summary>
        /// Realiza la autenticación del usuario a través de una petición HTTP POST a la URL `https://localhost:44339/usuario/login`.
        /// </summary>
        /// <param name="model">Los datos del usuario que está intentando iniciar sesión.</param>
        /// <returns>Redirige al usuario a la página de inicio si la autenticación es exitosa, o muestra un mensaje de error en la página de inicio de sesión si la autenticación falla.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                // Envía la solicitud HTTP POST al endpoint /login del controlador de backend
                StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"https://localhost:44339/usuario/login", content);

                if (response.IsSuccessStatusCode)
                {
                    // Si la solicitud es exitosa, obtiene el token de autenticación
                    var token = await response.Content.ReadAsStringAsync();

                    // Decodifica el token para obtener los datos de usuario, como el rol
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);
                    var role = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role")?.Value;
                    var NameIdentifier = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;

                    // Guarda el token y el rol en la sesión
                    HttpContext.Session.SetString("Token", token);
                    HttpContext.Session.SetString("Role", role);
                    HttpContext.Session.SetString("TokenId", NameIdentifier);

                    // Establece la cookie de autenticación y redirecciona al usuario a la página de inicio
                    SetAuthCookies(token, role, NameIdentifier);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Si la solicitud falla, muestra un mensaje de error en la página de inicio de sesión
                    ModelState.AddModelError("", "Nombre de usuario o contraseña incorrecta");
                    ViewBag.ErrorMessage = "Usuario Inactivo";
                    return View(model);
                }
            }
            catch (HttpRequestException ex)
            {
                // Si ocurre un error de conexión, se muestra un mensaje de error en la vista.
                TempData["ErrorMessage"] = "Ocurrió un error al intentar conectar con la API. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error. Por favor, inténtelo de nuevo más tarde." + ex;
                return View("~/Views/Home/Index.cshtml");
            }
        }

        /// <summary>
        /// Realiza la eliminación de las cookies de autenticación y cualquier otro token o información de sesión almacenados en la aplicación.
        /// </summary>
        /// <returns>Redirige al usuario a la página de inicio de sesión.</returns>
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Elimina la cookie de autenticación y cualquier otro token o información de sesión almacenados en la aplicación
            HttpContext.Response.Cookies.Delete("Token");
            HttpContext.Response.Cookies.Delete("TokenRole");
            HttpContext.Response.Cookies.Delete("TokenId");

            // Redirecciona al usuario a la página de inicio de sesión
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Devuelve la vista correspondiente a la página de inicio de sesión.
        /// </summary>
        /// <returns>La vista correspondiente a la página de inicio de sesión.</returns>
        public IActionResult InterfazLogin()
        {
            return View("~/Views/Usuario/Login.cshtml");
        }

        /// <summary>
        /// Metodo que devuelve la vista de recuperar contraseña
        /// </summary>
        /// <returns>La vista correspondiente a la página de ingresar correo para recuperar contraseña.</returns>
        public IActionResult InterfazRecuperar()
        {
            return View("~/Views/Usuario/RecuperarContrasena.cshtml");
        }

        /// <summary>
        /// Metodo que envía los datos mediante un HttpClient a un endpoint Patch que actualizará la contraseña del usuario
        /// </summary>
        /// <param name="datosRecuperar"></param>
        /// <returns>vista de login con los datos actualizados</returns>
        public async Task<IActionResult> RecuperarContrasena(RecuperaContrasenaModel datosRecuperar)
        {
            try
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(datosRecuperar), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"https://localhost:44339/usuario/recupera", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var resultId = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    // Aquí guardas el código de recuperación en una variable
                    datosRecuperar.CodigoRecuperacion = resultId.ToString(); // Convertir a cadena

                    // Retorna una vista con la variable de código de recuperación
                    return View("~/Views/Usuario/IngresaCodigo.cshtml", datosRecuperar);
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
        /// Metodo que devuelve la vista de actualizar contraseña
        /// </summary>
        /// <returns>La vista correspondiente a la página de ingresar correo para recuperar contraseña.</returns>
        public IActionResult InterfazActualizar(RecuperaContrasenaModel request)
        {
            return View("~/Views/Usuario/ActualizarContrasena.cshtml", request);
        }


        public async Task<IActionResult> ActualizarContrasena(RecuperaContrasenaModel request)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _client.PatchAsync($"https://localhost:44339/usuario/", content);

                return InterfazLogin();
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
        /// Método auxiliar para establecer las cookies de autenticación.
        /// </summary>
        /// <param name="token">El token de autenticación del usuario.</param>
        /// <param name="role">El rol del usuario autenticado.</param>
        /// <param name="nameIdentifier">El identificador único del usuario autenticado.</param>
        private void SetAuthCookies(string token, string role, string nameIdentifier)
        {
            // Establece las cookies de autenticación con una duración de 30 minutos, HttpOnly activado, SameSite en Strict y Secure en true.
            HttpContext.Response.Cookies.Append("Token", token, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            });

            HttpContext.Response.Cookies.Append("TokenRole", role, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            });

            HttpContext.Response.Cookies.Append("TokenId", nameIdentifier, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            });
        }
    }
}