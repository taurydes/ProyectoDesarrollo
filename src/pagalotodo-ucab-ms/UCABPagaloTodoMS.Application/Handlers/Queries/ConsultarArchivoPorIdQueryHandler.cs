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
    public class ConsultarArchivoPorIdQueryHandler : IRequestHandler<ConsultarArchivoPorIdQuery,  ArchivoConciliacionResponse>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ConsultarArchivoPorIdQueryHandler> _logger;
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConsultarArchivoPorIdQueryHandler"/>.
        /// </summary>
        /// <param name="dbContext">El contexto de la base de datos.</param>
        /// <param name="logger">El registrador.</param>
        public ConsultarArchivoPorIdQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarArchivoPorIdQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja la consulta de ConsultarArchivoPorIdQuery y recupera un ArchivoResponse.
        /// </summary>
        /// <param name="request">La consulta de ConsultarArchivoPorIdQuery.</param>
        /// <param name="cancellationToken">El token de cancelación.</param>
        /// <returns>Una tarea que representa la operación asíncrona con el ArchivoResponse.</returns>
        public async Task<ArchivoConciliacionResponse> Handle(ConsultarArchivoPorIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
                {
                    _logger.LogWarning("ConsultarArchivoPorIdQueryHandler.Handle: Request nulo.");
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
                _logger.LogWarning("ConsultarArchivoPorIdQueryHandler.Handle: ArgumentNullException");
                throw;
            }
        }

        /// <summary>
        /// Maneja la operación asíncrona para recuperar un ArchivoConciliacionResponse por su ID.
        /// </summary>
        /// <param name="id">El ID del archivo.</param>
        /// <returns>Una tarea que representa la operación asíncrona con el ArchivoConciliacionResponse.</returns>
        private async Task<ArchivoConciliacionResponse> HandleAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("ConsultarArchivoPorIdQueryHandler.HandleAsync");

                var _archivo = await _dbContext.ArchivoConciliacions.FindAsync(id);

                if (_archivo is null)
                {
                    throw new CampoIdNullException("No se encontró el campo con el ID especificado.");
                }

                var response = ArchivoConciliacionMapper.MapEntityResponse(_archivo);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ConsultarArchivoPorIdQueryHandler.HandleAsync. {Mensaje}", ex.Message);
                throw;
            }
        }

        
    }
}

