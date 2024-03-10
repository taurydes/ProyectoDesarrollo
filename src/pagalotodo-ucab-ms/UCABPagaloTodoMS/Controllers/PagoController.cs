using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Drawing;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeaturePago;
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
    /// Controlador para las operaciones relacionadas con los pagos.
    /// </summary>
    [ApiController]
    [Route("[controller]")]


    public class PagoController : BaseController<PagoController>
    {
        private readonly IMediator _mediator;
        private readonly PagoValidator _pagoValidator; // nueva

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public PagoController(ILogger<PagoController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;
            _pagoValidator = new PagoValidator();

        }

        /// <summary>
        /// Crea un nuevo pago.
        /// </summary>
        /// <param name="pago">Datos del pago a crear.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(CrearPagoRequest pago)
        {
            try
            {
                _logger.LogInformation("Entrando al método Create de PagoController");
               
                var validationResult = _pagoValidator.Validate(pago);
                if (!validationResult.IsValid)
                {
                    _logger.LogInformation("Entrando al validador en error ");

                    return BadRequest(validationResult.Errors);
                }
                // continúa ejecución
                var query = new CrearPagoCommand(pago);
                var response = await _mediator.Send(query);
                
                return Ok(response);
            }
            catch (PagoNullException ex)
            {
                _logger.LogError(ex, "Ocurrió un error en CrearPago. PagoNullException: {Mensaje}", ex.Message);
                return BadRequest($"Ocurrió un error en CrearPago: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en Create de PagoController", ex);
                return BadRequest($"Ocurrió un error en Create de PagoController: {StatusCodes.Status400BadRequest}");
            }
        }
                
    }
}
