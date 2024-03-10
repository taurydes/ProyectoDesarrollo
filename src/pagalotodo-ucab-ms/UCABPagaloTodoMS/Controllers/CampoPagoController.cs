using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Drawing;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureCampoPago;
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
    /// clase controladora de los campos asociados a un pago, dependerán de la configuración de pagos
    /// </summary>
    [ApiController]
    [Route("[controller]")]

    public class CampoPagoController : BaseController<CampoPagoController>
    {
        private readonly IMediator _mediator;
        private readonly CampoPagoValidator _campoPagoValidator; // nueva


        /// <summary>
        /// metodo contructor de la clase
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public CampoPagoController(ILogger<CampoPagoController> logger, IMediator mediator) : base(logger)
        {
            _mediator = mediator;
            _campoPagoValidator = new CampoPagoValidator();

        }

        /// <summary>
        /// Método HTTP POST para crear un nuevo campo.
        /// </summary>
        /// <param name="campos">Datos del campo a crear.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(List<CrearCampoPagoRequest> campos)
        {
            try
            {
                _logger.LogInformation("Entrando al método Create de CampoPagoController");

                foreach (var item in campos)
                {

                    var validationResult = _campoPagoValidator.Validate(item);
                    if (!validationResult.IsValid)
                    {
                        _logger.LogInformation("Entrando al validador en error ");

                        return BadRequest(validationResult.Errors);
                    }
                    // continúa ejecución
                        var query = new CrearCampoPagoCommand(item);
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
                _logger.LogError("Ocurrió un error en Create de CampoPagoController", ex);
                return BadRequest($"Ocurrió un error en Create de CampoPagoController: {StatusCodes.Status400BadRequest}");
            }
        }

        /// <summary>
        /// Método HTTP GET para consultar todos los Campos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<CampoPagoResponse>>> ConsultarCamposQuery()
        {
            _logger.LogInformation("Entrando al método ConsultarUsuariosQuery de ConsumidorController");
            try
            {
                var query = new ConsultarCamposQuery();
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
                _logger.LogError("Ocurrió un error en ConsultarConsumidorQuery de ConsumidorController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarConsumidorQuery de ConsumidorController.");
            }


        }
    }
}
