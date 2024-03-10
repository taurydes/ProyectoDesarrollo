using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Drawing;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureConsumidor;
using UCABPagaloTodoMS.Application.Commands.FeaturePrestadorServicio;
using UCABPagaloTodoMS.Application.Handlers.Queries;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Application.Validators;
using UCABPagaloTodoMS.Base;
using UCABPagaloTodoMS.Core.Entities;
using UCABPagaloTodoMS.Application.Services;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using System.Diagnostics.CodeAnalysis;
using OtpSharp;
using Wiry.Base32;
using System.Net.Mail;
using UCABPagaloTodoMS.Application.Exceptions;

namespace UCABPagaloTodoMS.Controllers
{
    /// <summary>
    /// Controlador para la gestión de usuarios.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : BaseController<UsuarioController>
    {
        private readonly IMediator _mediator;
        private readonly IJwtService _jwtService;// nueva
        private readonly IMapper _mapper;
        private readonly IConfiguration _config; // Agrega esta línea 

        /// <summary>
        /// Constructor de la clase UsuarioController.
        /// </summary>
        /// <param name="logger">Instancia del logger.</param>
        /// <param name="mediator">Instancia del mediator.</param>
        /// <param name="jwtService">Instancia del servicio de generación de tokens JWT.</param>
        /// <param name="mapper">Instancia del mapeador AutoMapper.</param>
        /// <param name="config">Instancia de la configuración.</param>
        public UsuarioController(ILogger<UsuarioController> logger, IMediator mediator, IJwtService jwtService, IMapper mapper, IConfiguration config) : base(logger)
        {
            _mediator = mediator;
            _jwtService = jwtService;
            _mapper = mapper;
            _config = config; // Asigna la dependencia a la propiedad _config
        }



        /// <summary>
        /// Método para autenticar un usuario y generar un token JWT.
        /// </summary>
        /// <param name="loginRequest">Datos de inicio de sesión.</param>
        /// <returns>Token JWT si la autenticación es exitosa.</returns>
        [ExcludeFromCodeCoverage]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Login(LoginRequest loginRequest)
        {
            try
            {
                var usuarioResponse = await _mediator.Send(new ConsultarUsuarioPorNombreQuery(loginRequest.NombreUsuario));

                if (usuarioResponse == null)
                {
                    return BadRequest("Usuario Inactivo");
                }
                if (usuarioResponse.Clave != loginRequest.Clave)
                {
                    return BadRequest("Nombre de usuario o contraseña incorrecta");
                } 
                var usuario = _mapper.Map<Usuario>(usuarioResponse);
                //if (usuario == null)
                //{
                //    return BadRequest("Nombre de usuario o contraseña incorrecta");
                //}

                // Crea una lista de roles para el usuario
                var roles = new List<string>();

                switch (usuarioResponse)
                {
                    case ConsumidorResponse :
                        roles.Add("CONSUMIDOR");
                        break;
                    case PrestadorServicioResponse :
                        roles.Add("PRESTADORSERVICIO");
                        break;
                    case AdministradorResponse :
                        roles.Add("ADMINISTRADOR");
                        break;
                    default:
                        // Lanza una excepción si el tipo de usuario no se reconoce.
                        throw new UsuarioTipoNoReconocidoException("Tipo de usuario no reconocido.");
                }

                // Genera el token de autenticación y agrega la información del rol
                var jwtSecret = _config["JwtOptions:Secret"];
                if (string.IsNullOrEmpty(jwtSecret))
                {
                    throw new JwtSecretNullException("La clave secreta JWT no está configurada.");
                }
                var token = _jwtService.GenerateJwtToken(usuario, roles);

                return Ok(token);
            }
            catch (UsuarioNotFoundException ex)
            {
                _logger.LogError(ex, "Ocurrió un error en Create de UsuarioController");
                return BadRequest($"Ocurrió un error en Create de UsuarioController: {StatusCodes.Status400BadRequest}. {ex.Message}");
            }
            
            catch (UsuarioTipoNoReconocidoException ex)
            {
                _logger.LogError(ex, "Ocurrió un error en Create de UsuarioController");
                return BadRequest($"Ocurrió un error en Create de UsuarioController: {StatusCodes.Status400BadRequest}. {ex.Message}");
            }
            catch (JwtSecretNullException ex)
            {
                _logger.LogError(ex, "Ocurrió un error en Create de UsuarioController");
                return BadRequest($"Ocurrió un error en Create de UsuarioController: {StatusCodes.Status400BadRequest}. {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en Create de UsuarioController");
                return BadRequest($"Ocurrió un error en Create de UsuarioController: {StatusCodes.Status400BadRequest}" + ex);
            }
        }

        /// <summary>
        /// Método para recuperar contraseña de losusuarios.
        /// </summary>
        /// <returns> string con el mensaje y envía el correo electronico con el código.</returns>
        [ExcludeFromCodeCoverage]
        [HttpPost("recupera")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RecuperarContrasena(RecuperaContraRequest recuperaRequest)
        {
            try
            {
                var correoExiste = await _mediator.Send(new ConsultarUsuarioPorCorreoQuery(recuperaRequest.Correo));
               
                if (correoExiste is null)
                {
                    return BadRequest("El correo electronico no se encuentra");
                }
                var response = await _mediator.Send(new RecuperaContrasenaCommand(recuperaRequest));
                
                if(response is null) return Ok("No hay conexión a internet");
                return Ok(response);
            
              
            }
            catch (RequestNullException ex)
            {
                _logger.LogWarning("Ocurrió una RequestNullException en ConsultarUsuariosQuery de UsuarioController. Exception: " + ex);
                return BadRequest("La solicitud es nula.");
            }
            catch (UsuarioNotFoundException ex)
            {
                _logger.LogError("UsuarioNotFoundException en ConsultarUsuarioPorCorreoQueryHandler de UsuarioController. Exception: " + ex);
                return NotFound("No se encontró ningún usuario con el correo electrónico proporcionado.");
            }
            catch (CorreoElectronicoNullException ex)
            {
                _logger.LogError("CorreoElectronicoNullException en ConsultarUsuarioPorCorreoQueryHandler de UsuarioController. Exception: " + ex);
                return BadRequest("El correo electrónico no puede ser nulo o vacío.");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                Console.WriteLine($"Response status code: {ex.StatusCode}");
                Console.WriteLine($"Response status code category: {ex.StatusCode}");
                Console.WriteLine($"Response from server: {ex.ToString()}");
                return BadRequest("Falla en smtpServer ");

            }
        }

        /// <summary>
        /// Método para consultar todos los usuarios.
        /// </summary>
        /// <returns>Lista de usuarios.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<UsuarioResponse>>> ConsultarUsuariosQuery()
        {
            _logger.LogInformation("Entrando al método ConsultarUsuariosQuery de UsuarioController");
            try
            {

                var query = new ConsultarUsuariosQuery();
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (RequestNullException ex)
            {
                _logger.LogWarning("Ocurrió una RequestNullException en ConsultarUsuariosQuery de UsuarioController. Exception: " + ex);
                return BadRequest("La solicitud es nula.");
            }
            catch (Exception ex)
            {
               _logger.LogError("Ocurrió un error en ConsultarUsuariosQuery de UsuarioController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarUsuariosQuery de UsuarioController.");
            }


        }

        /// <summary>
        /// Método para consultar un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>Usuario encontrado.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsuarioResponse>> ConsultarUsuarioPorIdQueryHandler(Guid id)
        {
            _logger.LogInformation("Entrando al método ConsultarUsuarioPorIdQuery de UsuarioController");
            try
            {

                var query = new ConsultarUsuarioPorIdQuery(id); // aquí se pasa el valor del parámetro "id"
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (UsuarioNotFoundException ex)
            {
                _logger.LogError("UsuarioNotFoundException en ConsultarUsuarioPorIdQueryHandler. Exception: " + ex.Message);
                return NotFound("No se encontró ningún usuario con el ID especificado.");
            }
            catch (Exception ex)
            {
               _logger.LogError("Ocurrió un error en GetById de UsuarioController. Exception: " + ex);
                return BadRequest("Ocurrió un error en GetById de UsuarioController.");
            }
        }


        /// <summary>
        /// Método para eliminar un usuario.
        /// </summary>
        /// <param name="id">ID del usuario a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Entrando al método Delete de UsuarioController");
            try
            {
                await _mediator.Send(new EliminarUsuarioCommand(id));

                return Ok("usuario eliminado");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en Create de ServicioPrestadoController", ex);
                return BadRequest($"Ocurrió un error en Delete de UsuarioController: {StatusCodes.Status400BadRequest}");
            }
        }

        /// <summary>
        /// Método HTTP PATCH para actualizar la contraseña de un usuario.
        /// </summary>
        /// <param name="usuario_new">Datos actualizados del usuario.</param>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ActualizarContraseña(ContrasenaUpdateRequest usuario_new)
        {
            _logger.LogInformation("Entrando al método Update de UsuarioController");
            try
            {
                var query = new ActualizarContrasenaCommand(usuario_new);
                
                var response = await _mediator.Send(query);
               
                if (!response) return BadRequest("No se logró cambiar la contraseña");
               
                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en consumidorupdaterequest de ConsumidorController. Exception: " + ex);
                return BadRequest("Ocurrió un error en consumidorupdaterequest de ConsumidorController.");
            }
        }
    }
}
