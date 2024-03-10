using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Drawing;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureConsumidor;
using UCABPagaloTodoMS.Application.Commands.FeaturePrestadorServicio;
using UCABPagaloTodoMS.Application.Commands.FeatureServiciosPrestados;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Handlers.Queries;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Application.Validators;
using UCABPagaloTodoMS.Base;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace UCABPagaloTodoMS.Controllers
{
    /// <summary>
    /// clase controladora de usuario tipo Administrador
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AdministradorController : BaseController<AdministradorController>

    {
        private readonly IMediator _mediator;
        private readonly AdministradorValidator _AdministradorValidator; // nueva
        private readonly AdministradorUpdateValidator _AdministradorUpdateValidator; // nueva


        /// <summary>
        /// método constructor para clase administrador.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        /// <param name="dbContext"></param>
        public AdministradorController(ILogger<AdministradorController> logger, IMediator mediator, IUCABPagaloTodoDbContext dbContext) : base(logger)
        {
            _mediator = mediator;
            _AdministradorValidator = new AdministradorValidator(dbContext);
            _AdministradorUpdateValidator = new AdministradorUpdateValidator(dbContext);

        }

        /// <summary>
        /// método para consultar la lista de administradores registrados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<AdministradorResponse>>> ConsultarAdministradoresQuery()
        {
            _logger.LogInformation("Entrando al método ConsultarAdministradorQuy de UsuarioController");
            try
            {

                var query = new ConsultarAdministradoresQuery();
                var response = await _mediator.Send(query);
                return Ok(response);
            }
            catch (RequestNullException ex)
            {
                _logger.LogWarning("Ocurrió una RequestNullException en ConsultarAdministradorQuy de AdministradorController. Exception: " + ex);
                return BadRequest("La solicitud es nula.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarAdministradorQuery de AdministradorController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarAdministradorQuery de AdministradorController.");
            }

        }

        /// <summary>
        /// método para consultar un administrador por su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AdministradorResponse>> ConsultarAdministradoresPorIdQuery(Guid id)
        {
            _logger.LogInformation("Entrando al método ConsultarAdministradorPorIdQuery de UsuarioController");
            try
            {

                var query = new ConsultarAdministradoresPorIdQuery(id); // aquí se pasa el valor del parámetro "id"
                var response = await _mediator.Send(query);
                if (response == null)
                {
                    throw new AdministradorNotFoundException($"No se encontró ningún administrador con el ID: {id}");
                }

                return Ok(response);
            }
            catch (AdministradorNotFoundException ex)
            {
                _logger.LogError(ex, "Ocurrió una AdministradorNotFoundException en ConsultarAdministradoresPorIdQuery de AdministradorController. Message: {Message}", ex.Message);
                return NotFound($"No se encontró ningún administrador con el ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en ConsultarAdministradoresPorIdQuery de AdministradorController. Exception: " + ex);
                return BadRequest("Ocurrió un error en ConsultarAdministradoresPorIdQuery de AdministradorController.");
            }
        }

        /// <summary>
        /// método para crear un administrador
        /// </summary>
        /// <param name="administrador"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Create(AdministradorRequest administrador)
        {
            _logger.LogInformation("Entrando al método Create de UsuarioController");
            try
            {
               
                    var validationResult = await _AdministradorValidator.ValidateAsync(administrador);
                    if (!validationResult.IsValid)
                    {
                        _logger.LogInformation("Entrando al validador en error ");

                        return BadRequest(validationResult.Errors);
                    }

                    //cambiar esto dependiendo llamar a AgregarAdministradorCommand
                    var query = new AgregarAdministradorCommand(administrador);
                    var response = await _mediator.Send(query);// aqui guarda el objeto usuario retorna id
                    return Ok(response);
           
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error en Create de AdministradorController");
                return BadRequest($"Ocurrió un error en Create de AdministradorController: {StatusCodes.Status400BadRequest}");
            }
        }

        /// <summary>
        /// método para actualizar los datos de un administrados
        /// </summary>
        /// <param name="id"></param>
        /// <param name="administrador_new"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(Guid id, AdministradorUpdateRequest administrador_new)
        {
            _logger.LogInformation("Entrando al método Update de AdministradorController");
            try
            {

                _logger.LogInformation("Entrando al AdministradorUpdaterequest");

                if (id != administrador_new.Id)
                {
                    _logger.LogInformation("Entrando al AdministradorUpdaterequest pero no conincide ID");

                    return BadRequest();
                }

                var validationResult = await _AdministradorUpdateValidator.ValidateAsync(administrador_new);
                if (!validationResult.IsValid)
                {
                    _logger.LogInformation("Entrando al validador en error ");

                    return BadRequest(validationResult.Errors);
                }
                var query = new ActualizarAdministradorCommand(administrador_new, id);
                var response = await _mediator.Send(query);
                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error en AdministradorUpdaterequest de AdministradorController. Exception: " + ex);
                return BadRequest("Ocurrió un error en AdministradorUpdaterequest de AdministradorController.");
            }
        }
    }
}
