using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Encoding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Drawing;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureConsumidor;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Handlers.Queries;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Application.Services;
using UCABPagaloTodoMS.Application.Validators;
using UCABPagaloTodoMS.Base;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using UCABPagaloTodoMS.Infrastructure.Database;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace UCABPagaloTodoMS.Controllers
{ 
    /// <summary>
    /// clase controladora de usuarios tipo contumidor
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    
    public class ConsumidorController : BaseController<ConsumidorController>
    {
        private readonly IMediator _mediator;
        private readonly ConsumidorValidator _consumidorValidator; // Nuevo validador para la entidad Consumidor
        private readonly ConsumidorUpdateValidator _consumidorUpdateValidator; // Nuevo validador para la actualización de Consumidor
        private readonly IUCABPagaloTodoDbContext _dbcontext; // Contexto de base de datos

        /// <summary>
        /// contructor de la clase
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        /// <param name="dbContext"></param>
        public ConsumidorController(ILogger<ConsumidorController> logger, IMediator mediator, IUCABPagaloTodoDbContext dbContext) : base(logger)
        {
            _mediator = mediator;
            _consumidorValidator = new ConsumidorValidator(dbContext);
            _consumidorUpdateValidator = new ConsumidorUpdateValidator();
            _dbcontext = dbContext;
            

        }
        /// <summary>
        /// Método HTTP GET para consultar todos los consumidores.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ConsumidorResponse>>> ConsultarConsumidorQuery()
        {
            _logger.LogInformation("Entrando al método ConsultarUsuariosQuery de ConsumidorController");
            try
            {
                var query = new ConsultarConsumidoresQuery();
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (RequestNullException ex)
            {
                _logger.LogError(ex, "Request nulo en ConsultarConsumidoresQuery de ConsumidorController");
                return BadRequest("El request no puede ser nulo");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarConsumidorQuery de ConsumidorController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarConsumidorQuery de ConsumidorController.");
            }


        }

        /// <summary>
        /// Método HTTP GET para consultar un consumidor por su ID.
        /// </summary>
        /// <param name="id">ID del consumidor a consultar.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ConsumidorResponse>> ConsultarConsumidoresPorIdQuery(Guid id)
        {
            _logger.LogInformation("Entrando al método ConsultarUsuarioPorIdQuery de UsuarioController");
            try
            {
                var query = new ConsultarConsumidoresPorIdQuery(id); // aquí se pasa el valor del parámetro "id"
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (CampoIdNullException ex)
            {
                _logger.LogError(ex, $"No se encontró el campo con el ID: {id}");
                return NotFound($"No se encontró el campo con el ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en GetById de ConsumidorController. Exception: " + ex);
                return BadRequest($"Ocurrió un error en GetById de ConsumidorController : {ex.Message}");
            }
        }

        /// <summary>
        /// Método HTTP GET para consultar un consumidor por su cedula.
        /// </summary>
        /// <param name="id">cedula del consumidor a consultar.</param>
        [HttpGet]
        [Route("cedula/{cedula}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ConsumidorResponse>> ConsultarConsumidoresPorCedula(int cedula)
        {
            _logger.LogInformation("Entrando al método ConsultarUsuarioPorIdQuery de UsuarioController");
            try
            {
                var query = new ConsultarConsumidorPorCedulaQuery(cedula); // aquí se pasa el valor del parámetro "id"
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (RequestNullException ex)
            {
                _logger.LogError(ex, "Request nulo en ConsultarConsumidoresQuery de ConsumidorController");
                return BadRequest("El request no puede ser nulo");
            }
            catch (UsuarioNotFoundException ex)
            {
                _logger.LogError(ex, "EL usuario no puede ser nulo");
                return BadRequest("Usuario no puede ser nulo");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en GetById de ConsumidorController. Exception: " + ex);
                return BadRequest($"Ocurrió un error en GetById de ConsumidorController : {ex.Message}");
            }
        }



        /// <summary>
        /// Método HTTP POST para crear un nuevo consumidor.
        /// </summary>
        /// <param name="consumidor">Datos del consumidor a crear.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseService), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Create(ConsumidorRequest consumidor)
        {
            _logger.LogInformation("Entrando al método Create de UsuarioController");
            try
            {
                var validationResult = await _consumidorValidator.ValidateAsync(consumidor);
                if (!validationResult.IsValid)
                {
                    _logger.LogInformation("Entrando al validador en error");
                    var errorResponse = new ErrorResponseService
                    {
                        ErrorCode = 555,
                        ErrorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                    };
                    return BadRequest(errorResponse);
                }

                var query = new AgregarConsumidorCommand(consumidor);
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en Create de ConsumidorController");
                return BadRequest(new ErrorResponseService
                {
                    ErrorCode = 555,
                    ErrorMessages = new List<string> { $"Ocurrió un error en Create de ConsumidorController: {ex.Message}" }
                });
            }
        }

        /// <summary>
        /// Método HTTP PUT para actualizar un consumidor.
        /// </summary>
        /// <param name="id">ID del consumidor a actualizar.</param>
        /// <param name="consumidor_new">Datos actualizados del consumidor.</param>
        [HttpPut]
        [Route("actualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(Guid id, ConsumidorUpdateRequest consumidor_new)
        {
            _logger.LogInformation("Entrando al método Update de UsuarioController");
            try
            {
              
                    _logger.LogInformation("Entrando al consumidorupdaterequest");

                    if (id != consumidor_new.Id)
                    {
                        _logger.LogInformation("Entrando al consumidorupdaterequest pero no conincide ID");

                        return BadRequest();
                    }

                var validationResult = await _consumidorUpdateValidator.ValidateAsync(consumidor_new);
                if (!validationResult.IsValid)
                {
                    _logger.LogInformation("Entrando al validador en error");
                    return BadRequest(validationResult.Errors);
                }
                var query = new ActualizarConsumidorCommand(consumidor_new, id);
                    var response = await _mediator.Send(query);
                    return Ok(response);
                               
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en consumidorupdaterequest de ConsumidorController. Exception: " + ex);
                return BadRequest("Ocurrió un error en consumidorupdaterequest de ConsumidorController.");
            }
        }

        /// <summary>
        /// Método HTTP PUT para cambiar el estatus de un consumidor.
        /// </summary>
        /// <param name="id">ID del consumidor cuyo estatus se cambiará.</param>
        [HttpPut]
        [Route("estatus/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CambiaEstatus(Guid id)
        {
            _logger.LogInformation("Entrando al método Update de UsuarioController");
            try
            {
                var consumidorResponse = await _mediator.Send(new ConsultarConsumidoresPorIdQuery(id));

                _logger.LogInformation("Entrando al CambiarStatus");

                if (consumidorResponse == null)
                {
                    _logger.LogInformation("Entrando al CambiaEstatus pero no conincide ID");

                    return BadRequest("Ocurrió no se encuentra el Usuario.");
                }

                var query = new CambiarEstatusCommand(consumidorResponse, id);
                var response = await _mediator.Send(query);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en CambiaEstatus de ConsumidorController. Exception: " + ex);
                return BadRequest("Ocurrió un error en CambiaEstatus de ConsumidorController.");
            }
        }
    }
}
