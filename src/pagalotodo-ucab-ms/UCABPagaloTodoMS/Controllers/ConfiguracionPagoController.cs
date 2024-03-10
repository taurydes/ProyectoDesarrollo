using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Drawing;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureConfiguracionPago;
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
    /// clase controladora de configuración de pagos
    /// </summary>
    [ApiController]
    [Route("[controller]")]

    public class ConfiguracionPagoController : BaseController<ConfiguracionPagoController>
    {
        private readonly IMediator _mediator;
        private readonly PagoValidator _pagoValidator; // nueva


        /// <summary>
        /// constructor de la clase 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public ConfiguracionPagoController(ILogger<ConfiguracionPagoController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;
            _pagoValidator = new PagoValidator();

        }


        /// <summary>
        /// Método HTTP POST para crear una nueva configuracion de pago.
        /// </summary>
        /// <param name="configuracionPago">Datos de la configuracion de pago a crear.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(CrearConfiguracionPagoRequest configuracionPago)
        {
            try
            {
                _logger.LogInformation("Entrando al método Create de ConfiguracionPagoController");
               
                //var validationResult = _pagoValidator.Validate(pago);
                //if (!validationResult.IsValid)
                //{
                //    _logger.LogInformation("Entrando al validador en error ");

                //    return BadRequest(validationResult.Errors);
                //}
                // continúa ejecución
                var query = new CrearConfiguracionPagoCommand(configuracionPago);
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
                _logger.LogError("Ocurrió un error en Create de ConfiguracionPagoController", ex);
                return BadRequest($"Ocurrió un error en Create de ConfiguracionPagoController: {StatusCodes.Status400BadRequest}");
            }
        }

        /// <summary>
        /// Método HTTP GET para consultar una configuracion  de Pago por su ID.
        /// </summary>
        /// <param name="id">ID de la configuracion a consultar.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ConfiguracionPagoResponse>> ConsultarConfiguracionPagoPorIdQuery(Guid id)
        {
            _logger.LogInformation("Entrando al método ConsultarUsuarioPorIdQuery de UsuarioController");
            try
            {
                var query = new ConsultarConfiguracionPagoPorIdQuery(id); // aquí se pasa el valor del parámetro "id"
                var response = await _mediator.Send(query);

                // Agrega los encabezados CORS
                Response.Headers.Add("Access-Control-Allow-Origin", "*");
                Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-Requested-With");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarPrestadoresPorIdQuery de PrestadorServicioController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarPrestadoresPorIdQuery de PrestadorServicioController.");
            }
        }
    }
}
