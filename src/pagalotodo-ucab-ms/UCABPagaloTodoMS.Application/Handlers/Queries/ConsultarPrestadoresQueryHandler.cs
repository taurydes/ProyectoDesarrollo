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
    public class ConsultarPrestadoresQueryHandler : IRequestHandler<ConsultarPrestadoresQuery, List<PrestadorServicioResponse>>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ConsultarPrestadoresQueryHandler> _logger;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConsultarPrestadoresQueryHandler"/>.
        /// </summary>
        /// <param name="dbContext">El contexto de la base de datos.</param>
        /// <param name="logger">El registrador.</param>
        public ConsultarPrestadoresQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarPrestadoresQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja la consulta de ConsultarPrestadoresQuery y recupera una lista de PrestadorServicioResponse.
        /// </summary>
        /// <param name="request">La consulta de ConsultarPrestadoresQuery.</param>
        /// <param name="cancellationToken">El token de cancelación.</param>
        /// <returns>Una tarea que representa la operación asíncrona con la lista de PrestadorServicioResponse.</returns>
        public Task<List<PrestadorServicioResponse>> Handle(ConsultarPrestadoresQuery request, CancellationToken cancellationToken)
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
                _logger.LogWarning("El Request es nulo");
                throw;
            }
            catch (Exception)
            {
                _logger.LogWarning("ConsultarPrestadoresQueryHandler.Handle: ArgumentNullException");
                throw;
            }
        }

        /// <summary>
        /// Maneja la operación asíncrona para recuperar una lista de PrestadorServicioResponse.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona con la lista de PrestadorServicioResponse.</returns>
        private async Task<List<PrestadorServicioResponse>> HandleAsync()
        {
            try
            {
                _logger.LogInformation("ConsultarUsuariosQueryHandler.HandleAsync");
                var result = await _dbContext.PrestadorServicios.ToListAsync();

                var responseList = result.Select(entity => UsuarioMapper.MapEntityPrestadorServicioAResponse(entity)).ToList();

                return responseList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ConsultarUsuariosQueryHandler.HandleAsync. {Mensaje}", ex.Message);
                throw;
            }
        }

    }
}
