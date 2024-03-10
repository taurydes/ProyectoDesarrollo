using FluentAssertions.Common;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Drawing;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureServicioPrestado;
using UCABPagaloTodoMS.Application.Commands.FeatureServiciosPrestados;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Handlers.Queries;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Application.Validators;
using UCABPagaloTodoMS.Base;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace UCABPagaloTodoMS.Controllers
{
    /// <summary>
    /// Controlador para la gestión de los servicios prestados.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ServicioPrestadoController : BaseController<ServicioPrestadoController>
    {
        private readonly IMediator _mediator;
        private readonly ServicioPrestadoValidator _ServicioPrestadoValidator; // nueva
        private readonly ServicioPrestadoUpdateValidator _ServicioPrestadoUpdateValidator; // nueva

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public ServicioPrestadoController(ILogger<ServicioPrestadoController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;
            _ServicioPrestadoValidator = new ServicioPrestadoValidator();
            _ServicioPrestadoUpdateValidator = new ServicioPrestadoUpdateValidator();

        }

        /// <summary>
        /// Obtiene la lista de servicios prestados.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ServicioPrestadoResponse>>> ConsultarServiciosQuery()
        {
            _logger.LogInformation("Entrando al método ConsultarUsuariosQuery de ServicioPrestadoController");
            try
            {
                var query = new ConsultarServicioPrestadoQuery();
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarServiciosQuery de ServicioPrestadoController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarServiciosQuery de ServicioPrestadoController.");
            }
        }

        /// <summary>
        /// Obtiene un servicio prestado por su ID.
        /// </summary>
        /// <param name="id">ID del servicio prestado.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServicioPrestadoResponse>> ConsultarServicioPrestadoPorIdQuery(Guid id)
        {
            _logger.LogInformation("Entrando al método ConsultarServicioPrestadoPorIdQuery de ServicioPrestadoController");
            try
            {
                var query = new ConsultarServicioPrestadoPorIdQuery(id); // aquí se pasa el valor del parámetro "id"
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (ServicioPrestadoNotFoundException ex)
            {
                // Manejar la excepción específica de ServicioPrestadoNotFoundException
                // Puedes devolver un mensaje de error específico o realizar otras acciones

                _logger.LogError(ex, "ServicioPrestadoNotFoundException en ConsultarServicioPrestadoPorIdQuery de ServicioPrestadoController");
                return NotFound("No se encontró el servicio prestado con el ID proporcionado");
            }
            catch (RequestNullException ex)
            {
                // Manejar la excepción específica de ServicioPrestadoNotFoundException
                // Puedes devolver un mensaje de error específico o realizar otras acciones

                _logger.LogError(ex, "El Request es nulo");
                return NotFound("El Request es nulo");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarServicioPrestadoPorIdQuery de ServicioPrestadoController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarServicioPrestadoPorIdQuery de ServicioPrestadoController.");
            }
        }

        /// <summary>
        /// Crea un nuevo servicio prestado.
        /// </summary>
        /// <param name="servicio">Datos del servicio prestado a crear.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Create(CrearServicioPrestadoRequest servicio)
        {
            _logger.LogInformation("Entrando al método Create de ServicioPrestadoController");
            try
            {
                    _logger.LogInformation("Entrando al método Create de ServicioPrestadoController");
                   
                    var validationResult = await _ServicioPrestadoValidator.ValidateAsync(servicio);
                    if (!validationResult.IsValid)
                    {
                        _logger.LogInformation("Entrando al validador en error ");

                        return BadRequest(validationResult.Errors);
                    }
                    //cambiar esto dependiendo llamar a AgregarAdministradorCommand
                    var query = new CrearServicioPrestadoCommand(servicio);
                    var response = await _mediator.Send(query);// aqui guarda el objeto usuario retorna id
                    return Ok(response);
                  
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en Create de ServicioPrestadoController", ex);
                return BadRequest($"Ocurrió un error en Create de ServicioPrestadoController: {StatusCodes.Status400BadRequest}");
            }
        }

        /// <summary>
        /// Actualiza un servicio prestado existente.
        /// </summary>
        /// <param name="id">ID del servicio prestado a actualizar.</param>
        /// <param name="servicio_new">Nuevos datos del servicio prestado.</param>
        [HttpPut]
        [Route("actualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, ServicioPrestadoUpdateRequest servicio_new)
        {
            _logger.LogInformation("Entrando al método Update de ServicioPrestadoController");
            try
            {
               
                _logger.LogInformation("Entrando al consumidorupdaterequest");

                    if (id != servicio_new.Id)
                    {
                        _logger.LogInformation("Entrando al Update pero no conincide ID");

                        return BadRequest();
                    }

                    var validationResult = await _ServicioPrestadoUpdateValidator.ValidateAsync(servicio_new);
                    if (!validationResult.IsValid)
                    {
                        _logger.LogInformation("Entrando al validador en error ");

                        return BadRequest(validationResult.Errors);
                    }

                    var query = new ActualizarServicioPrestadoCommand(servicio_new, id);
                    var response = await _mediator.Send(query);
                    return Ok(response);
             
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en Update de ServicioPrestadoController. Exception: " + ex);
                return BadRequest("Ocurrió un error en Update de ServicioPrestadoController.");
            }
        }

        /// <summary>
        /// Elimina un servicio prestado.
        /// </summary>
        /// <param name="id">ID del servicio prestado a eliminar.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Entrando al método Delete de UsuarioController");
            try
            {
                await _mediator.Send(new EliminarServicioPrestadoCommand(id));

                return Ok("servicio eliminado");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en Delete de ServicioPrestadoController. Exception: " + ex);
                return BadRequest("Ocurrió un error en Delete de ServicioPrestadoController.");
            }
        }

        /// <summary>
        /// Cambia el estatus de un servicio prestado.
        /// </summary>
        /// <param name="id">ID del servicio prestado.</param>
        [HttpPut]
        [Route("estatus/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CambiaEstatus(Guid id)
        {
            _logger.LogInformation("Entrando al método Update de ServicioPrestadoController");
            try
            {
                var servicioResponse = await _mediator.Send(new ConsultarServicioPrestadoPorIdQuery(id));

                _logger.LogInformation("Entrando al CambiarStatus");

                if (servicioResponse == null)
                {
                    _logger.LogInformation("Entrando al CambiaEstatus pero no conincide ID");

                    return BadRequest("Ocurrió no se encuentra el Usuario.");
                }

                var query = new CambiarEstatusCommand(servicioResponse, id);
                var response = await _mediator.Send(query);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en CambiaEstatus de ServicioPrestadoController. Exception: " + ex);
                return BadRequest("Ocurrió un error en CambiaEstatus de ServicioPrestadoController.");
            }
        }

        [HttpPatch]
        [Route("configuracion/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AgregaConfiguracion(AgregaConfiguracionPagoRequest configuracion)
        {
            _logger.LogInformation("Entrando al método Update de ServicioPrestadoController");
            try
            {
                var servicioResponse = await _mediator.Send(new ConsultarServicioPrestadoPorIdQuery(configuracion.ServicioPrestadoId));
                
                _logger.LogInformation("Entrando al CambiarStatus");

                if (servicioResponse == null)
                {
                    _logger.LogInformation("Entrando al CambiaEstatus pero no conincide ID");

                    return BadRequest("Ocurrió no se encuentra el Usuario.");
                }

                var query = new AgregaConfiguracionCommand(servicioResponse.Id, configuracion.ConfiguracionPagoId);
                var response = await _mediator.Send(query);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en CambiaEstatus de ServicioPrestadoController. Exception: " + ex);
                return BadRequest("Ocurrió un error en CambiaEstatus de ServicioPrestadoController.");
            }
        }
    }
}
