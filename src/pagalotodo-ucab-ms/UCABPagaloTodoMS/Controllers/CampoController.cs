using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Drawing;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureCampo;
using UCABPagaloTodoMS.Application.Commands.FeatureConsumidor;
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
    /// clase controladora para los campos que tendran las configuraciones de pago
    /// </summary>
    [ApiController]
    [Route("[controller]")]

    public class CampoController : BaseController<CampoController>
    {
        private readonly IMediator _mediator;
        private readonly CampoValidator _campoValidator; // Nuevo validador para la entidad Campo
        private readonly CampoUpdateValidator _campoUpdateValidator; // Nuevo validador para la actualización de Campo


        /// <summary>
        /// metodo constructor de la clase.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public CampoController(ILogger<CampoController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;
            _campoValidator = new CampoValidator();
            _campoUpdateValidator = new CampoUpdateValidator();

        }

        /// <summary>
        /// Método HTTP POST para crear un nuevo campo.
        /// </summary>
        /// <param name="campos">Datos del campo a crear.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(List<CrearCampoRequest> campos)
        {
            try
            {
                _logger.LogInformation("Entrando al método Create de CampoController");

                foreach (var item in campos)
                {

                    var validationResult = _campoValidator.Validate(item);
                    if (!validationResult.IsValid)
                    {
                        _logger.LogInformation("Entrando al validador en error ");

                        return BadRequest(validationResult.Errors);
                    }

                    // continúa ejecución
                    var query = new CrearCampoCommand(item);
                    var response = await _mediator.Send(query);
                
                }
                
                return Ok();
            }
            catch (RequestNullException ex)
            {
                _logger.LogWarning("Ocurrió una RequestNullException" + ex);
                return BadRequest("La solicitud es nula.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en Create de CampoController", ex);
                return BadRequest($"Ocurrió un error en Create de CampoController: {StatusCodes.Status400BadRequest}");
            }
        }

        /// <summary>
        /// Método HTTP POST para crear un nuevo campo.
        /// </summary>
        /// <param name="campo">Datos del consumidor a crear.</param>
        [HttpPost]
        [Route("campo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateOne(CrearCampoRequest campo)
        {
            try
            {
                _logger.LogInformation("Entrando al método Create de CampoController");

              
                    var validationResult = _campoValidator.Validate(campo);
                    if (!validationResult.IsValid)
                    {
                        _logger.LogInformation("Entrando al validador en error ");

                        return BadRequest(validationResult.Errors);
                    }
                    // continúa ejecución
                    var query = new CrearCampoCommand(campo);
                    var response = await _mediator.Send(query);
                                

               return Ok();
            }
            catch (RequestNullException ex)
            {
                _logger.LogWarning("Ocurrió una RequestNullException" + ex);
                return BadRequest("La solicitud es nula.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en Create de CampoController", ex);
                return BadRequest($"Ocurrió un error en Create de CampoController: {StatusCodes.Status400BadRequest}");
            }
        }

        /// <summary>
        /// Método HTTP GET para consultar todos los Campos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<CampoResponse>>> ConsultarCamposQuery()
        {
            _logger.LogInformation("Entrando al método ConsultarCampos de CampoController");
            try
            {
                var query = new ConsultarCamposQuery();
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (CamposNotFoundException ex)
            {
                _logger.LogError(ex, "Error al consultar campos. No se encontraron campos.");
                return BadRequest("Error al consultar campos. No se encontraron campos.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarCampoQuery de CampoController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarCampoQuery de CampoController.");
            }


        }

        /// <summary>
        /// Método HTTP GET para consultar un campos dependiendo su ID.
        /// </summary>
        /// <param name="id">ID del consumidor a consultar.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CampoResponse>> ConsultarCampoPorIdQuery(Guid id)
        {
            _logger.LogInformation("Entrando al método ConsultarcampoPorIdQuery de campoController");
            try
            {
                var query = new ConsultarCamposPorIdQuery(id); // aquí se pasa el valor del parámetro "id"
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (CampoIdNullException ex)
            {
                _logger.LogError(ex, "Error al consultar el campo por ID. El ID del campo es nulo.");
                return BadRequest("El ID del campo es nulo.");
            }
            catch (RequestNullException ex)
            {
                _logger.LogError(ex, "El Resquest no puede ser nulo.");
                return BadRequest("El Request es nulo.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarcampoPorIdQuery de campoController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarcampoPorIdQuery de campoController.");
            }
        }

        /// <summary>
        /// Método HTTP Delete para eliminar por completo un campo.
        /// </summary>
        /// <param name="id">ID del campo cuyo que se eliminará.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Entrando al método Delete de CampoController");
            try
            {
                await _mediator.Send(new EliminarCampoCommand(id));

                return Ok("Campo eliminado");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en Create de CampoController", ex);
                return BadRequest($"Ocurrió un error en Delete de CampoController: {StatusCodes.Status400BadRequest}");
            }
        }

        /// <summary>
        /// Método HTTP PUT para actualizar un consumidor.
        /// </summary>
        /// <param name="id">ID del consumidor a actualizar.</param>
        /// <param name="campo_new">Datos actualizados del consumidor.</param>
        [HttpPut]
        [Route("actualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(Guid id, CampoUpdateRequest campo_new)
        {
            _logger.LogInformation("Entrando al método Update de CampoController");
            try
            {

                _logger.LogInformation("Entrando al CampoUpdaterequest");

                if (id != campo_new.Id)
                {
                    _logger.LogInformation("Entrando al CampoUpdaterequest pero no conincide ID");

                    return BadRequest();
                }

                var validationResult = await _campoUpdateValidator.ValidateAsync(campo_new);
                if (!validationResult.IsValid)
                {
                    _logger.LogInformation("Entrando al validador en error");
                    return BadRequest(validationResult.Errors);
                }
                var query = new ActualizarCampoCommand(campo_new, id);
                var response = await _mediator.Send(query);
                return Ok(response);

            }
            catch (RequestNullException ex)
            {
                _logger.LogWarning("Ocurrió una RequestNullException" + ex);
                return BadRequest("La solicitud es nula.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en CampoUpdaterequest de CampoController. Exception: " + ex);
                return BadRequest("Ocurrió un error en CampoUpdaterequest de CampoController.");
            }
        }

    }
}
