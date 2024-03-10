using MediatR;
using Microsoft.AspNetCore.Mvc;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Application.Validators.UCABPagaloTodoMS.Application.Validators;
using UCABPagaloTodoMS.Base;
using UCABPagaloTodoMS.Application.Commands.FeatureArchivoConciliacion;
using UCABPagaloTodoMS.Infrastructure.Services;
using System.Text.Json;
using UCABPagaloTodoMS.Application.Handlers.Queries;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Exceptions;

namespace UCABPagaloTodoMS.Controllers
{   /// <summary>
    /// clase controladora de ARchivos de conciliación
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ArchivoConciliacionController : BaseController<ArchivoConciliacionController>
    {
        private readonly IMediator _mediator;
        private readonly IRabbitMQProducer _rabbitMQProducer;
       


        /// <summary>
        /// Constructor de la clase ArchivoConciliacionController.
        /// </summary>
        /// <param name="logger">Instancia de ILogger utilizada para registrar los mensajes de registro.</param>
        /// <param name="mediator">Instancia de IMediator utilizada para la comunicación entre controladores y servicios.</param>
        /// <param name="rabbitMQProducer">Instancia de IMediator utilizada para la comunicación entre controladores y servicios.</param>
        public ArchivoConciliacionController(ILogger<ArchivoConciliacionController> logger, IMediator mediator, IRabbitMQProducer rabbitMQProducer) : base(logger)
        {
            _mediator = mediator;
            _rabbitMQProducer = rabbitMQProducer;
        }

        /// <summary>
        /// Obtiene un servicio filtrando los pagos dentro de un rango de fechas.
        /// </summary>
        /// <param name="_servicioRango">Datos del servicio y rango de fechas a consultar.</param>
        /// <returns>Respuesta HTTP 200 OK con la consulta de pagos dentro del rango especificado.</returns>
        [HttpPost]
        [Route("Rango")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServicioPrestadoPagosRangoResponse>> ConsultarServicioPorRangoPagosQuery(PagosServicioPrestadoPorRangoRequest _servicioRango)
        {
            _logger.LogInformation("Entrando al método ConsultarArchivoConciliacionPorIdQuery de UsuarioController");
            try
            {
                var query = new ConsultarPagosServicioPrestadoPorRangoQuery(_servicioRango); // aquí se pasa el valor del parámetro "id"
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
                _logger.LogError("Ocurrió un error en ConsultarArchivoConciliacionPorIdQuery de ArchivoConcilacionController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarArchivoConciliacionPorIdQuery de ArchivoConcilacionController.");
            }

        }

        /// <summary>
        /// Obtiene un servicio filtrando los pagos dentro de un rango de fechas.
        /// </summary>
        /// <param name="_servicioRango">Datos del servicio y rango de fechas a consultar.</param>
        /// <returns>Respuesta HTTP 200 OK con la consulta de pagos dentro del rango especificado.</returns>
        [HttpPost]
        [Route("detalle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServicioPrestadoPagosRangoResponse>> CreaArchivoCierreContableDetalles(PagosServicioPrestadoPorRangoDetalleRequest _servicioRango)
        {
            _logger.LogInformation("Entrando al método ConsultarArchivoConciliacionPorIdQuery de UsuarioController");
            try
            {
                var query = new ConsultarPagosServicioPrestadoPorRangoDetalleQuery(_servicioRango);
                var response = await _mediator.Send(query);

                var archivo = new GuardarRespuestaEnArchivoDeTextoCommand(response);
                var responseArchivo = await _mediator.Send(archivo);

                return Ok(responseArchivo);
            }
            catch (RequestNullException ex)
            {
                _logger.LogWarning("Ocurrió una RequestNullException" + ex);
                return BadRequest("La solicitud es nula.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarArchivoConciliacionPorIdQuery de ArchivoConcilacionController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarArchivoConciliacionPorIdQuery de ArchivoConcilacionController.");
            }

        }


        /// <summary>
        /// Metodo para generar el cierre contable de archivo conciliación
        /// </summary>
        /// <param name="_servicioRango"></param>
        /// <returns>retorna la respuesta del mediador</returns>
        [HttpPost]
        [Route("Archivo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServicioPrestadoPagosRangoResponse>> CreaArchivoCierreContable(PagosServicioPrestadoPorRangoRequest _servicioRango)
        {
            _logger.LogInformation("Entrando al método ConsultarArchivoConciliacionPorIdQuery de UsuarioController");
            try
            {
                var query = new ConsultarPagosServicioPrestadoPorRangoQuery(_servicioRango); // aquí se pasa el valor del parámetro "id"
                var response = await _mediator.Send(query);

                // aquí se llama al nuevo método para guardar la respuesta en un archivo de texto
                var archivo = new GuardarRespuestaEnArchivoDeTextoCommand(response);
                var responseArchivo = await _mediator.Send(archivo);
               
                return Ok(responseArchivo);
            }
            catch (RequestNullException ex)
            {
                _logger.LogWarning("Ocurrió una RequestNullException" + ex);
                return BadRequest("La solicitud es nula.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarArchivoConciliacionPorIdQuery de ArchivoConcilacionController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarArchivoConciliacionPorIdQuery de ArchivoConcilacionController.");
            }
        }

        /// <summary>
        /// metodo que actualiza el estado de los pagos mediante el archivo de conciliacion
        /// </summary>
        /// <param name="datosRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Rabbit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> ProcesarRabbitDatosColaRabbitMQ(ActualizaEstadosPagoRabbitRequest datosRequest)
        {
            _logger.LogInformation("Entrando al método ConsultarArchivoConciliacionPorIdQuery de UsuarioController");
            try
            {
                foreach (var pago in datosRequest.Datos)
                {
                    // Convertir la lista de pagos a formato JSON
                    var message = JsonSerializer.Serialize(pago);

                    //Enviar el mensaje a la cola de RabbitMQ
                    
                    _rabbitMQProducer.PublishMessageToConciliacion_Queue(message);
                }

                return Ok(true);
            }
            catch (RequestNullException ex)
            {
                _logger.LogWarning("Ocurrió una RequestNullException" + ex);
                return BadRequest("La solicitud es nula.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarArchivoConciliacionPorIdQuery de ArchivoConcilacionController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarArchivoConciliacionPorIdQuery de ArchivoConciliacionController.");
            }
        }


        /// <summary>
        /// Metodo para generar el cierre contable de archivo conciliación
        /// </summary>
        /// <param name="_servicioRango"></param>
        /// <returns>retorna la respuesta del mediador</returns>
        [HttpPost]
        [Route("aprobado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> CreaArchivoCierreContableAprobado(PagosServicioPrestadoPorRangoDetalleRequest _servicioRango)
        {
            _logger.LogInformation("Entrando al método ConsultarArchivoConciliacionPorIdQuery de UsuarioController");
            try
            {
                var query = new ConsultarPagosAprobadoQuery(_servicioRango); // aquí se pasa el valor del parámetro "id"
                var response = await _mediator.Send(query);

                // aquí se llama al nuevo método para guardar la respuesta en un archivo de texto
                var archivo = new GuardarRespuestaEnArchivoDeTextoAprobadoCommand(response);
                var responseArchivo = await _mediator.Send(archivo);

                AgregarArchivoConciliacionRequest datosArchivo = new();
                datosArchivo.ServicioPrestadoId = _servicioRango.ServicioPrestadoId;
                datosArchivo.UrlDescarga = responseArchivo;

                var archivoEntity = new AgregarArchivoConciliacionCommand(datosArchivo);
                var responsearchivoEntity = await _mediator.Send(archivoEntity);

                return Ok(responseArchivo);
            }
            catch (RequestNullException ex)
            {
                _logger.LogWarning("Ocurrió una RequestNullException" + ex);
                return BadRequest("La solicitud es nula.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarArchivoConciliacionPorIdQuery de ArchivoConcilacionController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarArchivoConciliacionPorIdQuery de ArchivoConcilacionController.");
            }
        }

        /// <summary>
        /// Método HTTP PATCH para cambiar el estatus de un de uyn archivo de conciliación.
        /// </summary>
        /// <param name="id">ID del archivo de conciliación.</param>
        [HttpPatch]
        [Route("estatus/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CambiaEstatus(Guid id)
        {
            _logger.LogInformation("Entrando al método Update de UsuarioController");
            try
            {
                var _archivoResponse = await _mediator.Send(new ConsultarArchivoPorIdQuery(id));

                _logger.LogInformation("Entrando al CambiarStatus");

                if (_archivoResponse == null)
                {
                    _logger.LogInformation("Entrando al CambiaEstatus pero no conincide ID");

                    return BadRequest("Ocurrió no se encuentra el Usuario.");
                }

                var query = new CambiarEstatusCommand(_archivoResponse, id);
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
                _logger.LogError("Ocurrió un error en CambiaEstatus de ConsumidorController. Exception: " + ex);
                return BadRequest("Ocurrió un error en CambiaEstatus de ConsumidorController.");
            }
        }
    }
}
