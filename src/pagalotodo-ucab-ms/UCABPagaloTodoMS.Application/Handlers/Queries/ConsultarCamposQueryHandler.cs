using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Core.Entities;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Exceptions;

namespace UCABPagaloTodoMS.Application.Handlers.Queries
{
    public class ConsultarCamposQueryHandler : IRequestHandler<ConsultarCamposQuery, List<CampoResponse>>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ConsultarCamposQueryHandler> _logger;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConsultarCamposQueryHandler"/>.
        /// </summary>
        /// <param name="dbContext">El contexto de la base de datos.</param>
        /// <param name="logger">El registrador.</param>
        public ConsultarCamposQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarCamposQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja la consulta de ConsultarCamposQuery y recupera una lista de CampoResponse.
        /// </summary>
        /// <param name="request">La consulta de ConsultarCamposQuery.</param>
        /// <param name="cancellationToken">El token de cancelación.</param>
        /// <returns>Una tarea que representa la operación asíncrona con la lista de CampoResponse.</returns>
        public Task<List<CampoResponse>> Handle(ConsultarCamposQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
                {
                    _logger.LogWarning("ConsultarPrestadoresQueryHandler.Handle: Request nulo.");
                    throw new RequestNullException("El Request es nulo");
                }
                else
                {
                    return HandleAsync();
                }
            }
            catch (RequestNullException ex)
            {
                _logger.LogError(ex, "Request nulo.");
                throw; // Lanza la excepción personalizada hacia arriba para ser manejada en un nivel superior.
            }
            catch (Exception)
            {
                _logger.LogWarning("ConsultarPrestadoresQueryHandler.Handle: ArgumentNullException");
                throw;
            }
        }

        /// <summary>
        /// Maneja la operación asíncrona para recuperar una lista de CampoResponse.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona con la lista de CampoResponse.</returns>
        private async Task<List<CampoResponse>> HandleAsync()
        {
            try
            {
                _logger.LogInformation("ConsultarUsuariosQueryHandler.HandleAsync");

                var result = await _dbContext.Campos.ToListAsync();

                if (result.Count == 0)
                {
                    throw new CamposNotFoundException();
                }

                var responseList = result.Select(entity => CampoMapper.MapEntityAResponse(entity)).ToList();

                return responseList;
            }
            catch (CamposNotFoundException ex)
            {
                _logger.LogError(ex, "Error al consultar campos. No se encontraron campos.");
                throw; // Lanza la excepción personalizada hacia arriba para ser manejada en un nivel superior.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ConsultarUsuariosQueryHandler.HandleAsync. {Mensaje}", ex.Message);
                throw;
            }
        }

    }
}
