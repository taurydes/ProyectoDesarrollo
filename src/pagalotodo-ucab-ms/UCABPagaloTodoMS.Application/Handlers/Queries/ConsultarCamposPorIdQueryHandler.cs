using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Queries
{
    public class ConsultarCamposPorIdQueryHandler : IRequestHandler<ConsultarCamposPorIdQuery,  CampoResponse>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ConsultarCamposPorIdQueryHandler> _logger;
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConsultarCamposPorIdQueryHandler"/>.
        /// </summary>
        /// <param name="dbContext">El contexto de la base de datos.</param>
        /// <param name="logger">El registrador.</param>
        public ConsultarCamposPorIdQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarCamposPorIdQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja la consulta de ConsultarCamposPorIdQuery y recupera un ConsumidorResponse.
        /// </summary>
        /// <param name="request">La consulta de ConsultarConsumidoresPorIdQuery.</param>
        /// <param name="cancellationToken">El token de cancelación.</param>
        /// <returns>Una tarea que representa la operación asíncrona con el ConsumidorResponse.</returns>
        public async Task<CampoResponse> Handle(ConsultarCamposPorIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
                {
                    _logger.LogWarning("ConsultarCamposPorIdQueryHandler.Handle: Request nulo.");
                    throw new RequestNullException("El Request es nulo");
                }
                else
                {
                    return await HandleAsync(request.Id);
                }
            }
            catch (RequestNullException ex){ 
                _logger.LogWarning("El Request es nulo");
                throw;
            }
            catch (Exception)
            {
                _logger.LogWarning("ConsultarCamposPorIdQueryHandler.Handle: ArgumentNullException");
                throw;
            }
        }

        /// <summary>
        /// Maneja la operación asíncrona para recuperar un CampoResponse por su ID.
        /// </summary>
        /// <param name="id">El ID del consumidor.</param>
        /// <returns>Una tarea que representa la operación asíncrona con el CampoResponse.</returns>
        private async Task<CampoResponse> HandleAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("ConsultarCamposPorIdQueryHandler.HandleAsync");

                var campo = await _dbContext.Campos.FindAsync(id);

                if (campo is null)
                {
                    throw new CampoIdNullException("No se encontró el campo con el ID especificado.");
                }

                var response = CampoMapper.MapEntityAResponse(campo);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ConsultarCamposPorIdQueryHandler.HandleAsync. {Mensaje}", ex.Message);
                throw;
            }
        }

        
    }
}

