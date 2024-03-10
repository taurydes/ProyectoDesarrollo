using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Drawing;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeaturePrestadorServicio;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Handlers.Queries;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Application.Validators;
using UCABPagaloTodoMS.Base;
using UCABPagaloTodoMS.Core.Database;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace UCABPagaloTodoMS.Controllers
{
    /// <summary>
    /// Controlador para la gestión de prestadores de servicio.
    /// </summary>
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Route("[controller]")]
    public class PrestadorServicioController : BaseController<PrestadorServicioController>
    {
        private readonly IMediator _mediator;
        private readonly PrestadorServicioValidator _prestadorServicioValidator; // nueva
        private readonly PrestadorServicioUpdateValidator _prestadorServicioUpdateValidator; // nueva

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        /// <param name="dbContext"></param>
        public PrestadorServicioController(ILogger<PrestadorServicioController> logger, IMediator mediator, IUCABPagaloTodoDbContext dbContext) : base(logger)
        {
            _mediator = mediator;
            
            _prestadorServicioValidator = new PrestadorServicioValidator(dbContext);
            _prestadorServicioUpdateValidator = new PrestadorServicioUpdateValidator(dbContext);
        }


        /// <summary>
        /// Consulta todos los prestadores de servicio.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<PrestadorServicioResponse>>> ConsultarPrestadoresQuery()
        {
            _logger.LogInformation("Entrando al método ConsultarUsuariosQuery de PrestadorServicioController");
            try
            {

                var query = new ConsultarPrestadoresQuery();
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (RequestNullException ex)
            {
                _logger.LogError(ex, "Request nulo.");
                return BadRequest("Request nulo.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarPrestadoresQuery de PrestadorServicioController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarPrestadoresQuery de PrestadorServicioController.");
            }


        }

        /// <summary>
        /// Consulta un prestador de servicio por su ID.
        /// </summary>
        /// <param name="id">ID del prestador de servicio.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PrestadorServicioResponse>> ConsultarPrestadoresPorIdQuery(Guid id)
        {
            _logger.LogInformation("Entrando al método ConsultarUsuarioPorIdQuery de UsuarioController");
            try
            {
                var query = new ConsultarPrestadoresPorIdQuery(id); // aquí se pasa el valor del parámetro "id"
                var response = await _mediator.Send(query);

                // Agrega los encabezados CORS
                Response.Headers.Add("Access-Control-Allow-Origin", "*");
                Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-Requested-With");

                return Ok(response);
            }
            catch (RequestNullException ex)
            {
                _logger.LogError(ex, "Request nulo.");
                return BadRequest("Request nulo.");
            }
            catch (PrestadorServicioNotFoundException ex)
            {
                _logger.LogError(ex, "Error ConsultarUsuarioPorIdQueryHandler.HandleAsync. {Mensaje}", ex.Message);
                return BadRequest("No se encontro el prestador de servicio.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarPrestadoresPorIdQuery de PrestadorServicioController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarPrestadoresPorIdQuery de PrestadorServicioController.");
            }
        }
        /// <summary>
        /// Crea un nuevo prestador de servicio.
        /// </summary>
        /// <param name="prestador">Datos del prestador de servicio a crear.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Create(PrestadorServicioRequest prestador)
        {
            _logger.LogInformation("Entrando al método Create de PrestadorServicioController");
            try
            {
              
                    var validationResult = await _prestadorServicioValidator.ValidateAsync(prestador);
                    if (!validationResult.IsValid)
                    {
                        _logger.LogInformation("Entrando al validador en error ");

                        return BadRequest(validationResult.Errors);
                    }

                    //cambiar esto dependiendo llamar a AgregarAdministradorCommand
                    var query = new AgregarPrestadorServicioCommand(prestador);
                    var response = await _mediator.Send(query);// aqui guarda el objeto usuario retorna id
                    return Ok(response);
                                               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en Create de PrestadorServicioController");
                return BadRequest($"Ocurrió un error en Create de PrestadorServicioController: {StatusCodes.Status400BadRequest}");
            }
        }
        /// <summary>
        /// Actualiza un prestador de servicio existente.
        /// </summary>
        /// <param name="id">ID del prestador de servicio a actualizar.</param>
        /// <param name="prestador_new">Nuevos datos del prestador de servicio.</param>
        [HttpPut]
        [Route("actualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(Guid id, PrestadorServicioUpdateRequest prestador_new)
        {
            _logger.LogInformation("Entrando al método Update de PrestadorServicioController");
            try
            {

                _logger.LogInformation("Entrando al prestadorUpdaterequest");

                if (id != prestador_new.Id)
                {
                    _logger.LogInformation("Entrando al prestadorUpdaterequest pero no conincide ID");

                    return BadRequest();
                }

                var validationResult = await _prestadorServicioUpdateValidator.ValidateAsync(prestador_new);
                if (!validationResult.IsValid)
                {
                    _logger.LogInformation("Entrando al validador en error ");

                    return BadRequest(validationResult.Errors);
                }

                var query = new ActualizarPrestadorServicioCommand(prestador_new, id);
                var response = await _mediator.Send(query);
                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en prestadorUpdaterequest de PrestadorServicioController. Exception: " + ex);
                return BadRequest("Ocurrió un error en prestadorUpdaterequest de PrestadorServicioController.");
            }
        }

        /// <summary>
        /// Cambia el estado de un prestador de servicio.
        /// </summary>
        /// <param name="id">ID del prestador de servicio.</param>
        [HttpPut]
        [Route("estatus/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CambiaEstatus(Guid id)
        {
            _logger.LogInformation("Entrando al método Update de ServicioPrestadoController");
            try
            {
                var prestadorResponse = await _mediator.Send(new ConsultarPrestadoresPorIdQuery(id));

                _logger.LogInformation("Entrando al CambiarStatus");

                if (prestadorResponse == null)
                {
                    _logger.LogInformation("Entrando al CambiaEstatus pero no conincide ID");

                    return BadRequest("Ocurrió no se encuentra el Usuario.");
                }

                var query = new CambiarEstatusCommand(prestadorResponse, id);
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
