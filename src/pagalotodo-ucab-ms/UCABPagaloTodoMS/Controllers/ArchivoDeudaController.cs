using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureArchivoDeuda;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Base;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Controllers
{
    /// <summary>
    /// clase controladora de los archivos de deudas
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ArchivoDeudaController : BaseController<ArchivoDeudaController>

    {
        private readonly IMediator _mediator;
               
        public ArchivoDeudaController(ILogger<ArchivoDeudaController> logger, IMediator mediator, IUCABPagaloTodoDbContext dbContext) : base(logger)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Método HTTP POST para guardar la lista de consumidores que poseen deudas.
        /// </summary>
        /// <param name="deudores">Datos del campo a crear.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(List<AgregaDeudoresRequest> deudores)
        {
            try
            {
                _logger.LogInformation("Entrando al método Create de ArchivoDeudaController");
                if(deudores != null)
                {
                    await _mediator.Send(new EliminarRegistrosArchivoDeudaCommand(deudores[0].ServicioPrestadoId));
                }
                foreach (var item in deudores)
                {

                    var consumidoresponse = await _mediator.Send(new ConsultarConsumidorPorCedulaQuery(item.Cedula));
                    if(consumidoresponse != null)
                    {
                        item.ConsumidorId = consumidoresponse.Id;

                        var query = new AgregarDeudoresCommand(item);
                        var response = await _mediator.Send(query);
                    }

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
                _logger.LogError("Ocurrió un error en Create de ArchivoDeudaController", ex);
                return BadRequest($"Ocurrió un error en Create de ArchivoDeudaController: {StatusCodes.Status400BadRequest}");
            }
        }

        /// <summary>
        /// Método HTTP POST para consultar si un consumidor se encuentra en la tabla de archivos deuda de un servicio.
        /// </summary>
        /// <returns>Valor booleano sobre si el consumidor tiene una deuda con algún servicio.</returns>
        [HttpPost]
        [Route("consulta")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> ConsultarConsumidorArchivoDeuda(ConsumidorArchivoDeudaRequest consulta)
        {
            _logger.LogInformation("Entrando al método ConsultarUsuarioPorIdQuery de ArchivoDeudaController");
            try
            {

                var query = new ConsultarConsumidorArchivoDeudaQuery(consulta); // aquí se pasa el valor del parámetro para la consulta
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
                _logger.LogError("Ocurrió un error en GetById de ArchivoDeudaController. Exception: " + ex);
                return BadRequest("Ocurrió un error en GetById de ArchivoDeudaController.");
            }
        }
    }
}
