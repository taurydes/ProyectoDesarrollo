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
    public class ConsultarConsumidoresQueryHandler : IRequestHandler<ConsultarConsumidoresQuery, List<ConsumidorResponse>>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ConsultarConsumidoresQueryHandler> _logger;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConsultarConsumidoresQueryHandler"/>.
        /// </summary>
        /// <param name="dbContext">El contexto de la base de datos.</param>
        /// <param name="logger">El registrador.</param>
        public ConsultarConsumidoresQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarConsumidoresQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja la consulta de ConsultarConsumidoresQuery y recupera una lista de ConsumidorResponse.
        /// </summary>
        /// <param name="request">La consulta de ConsultarConsumidoresQuery.</param>
        /// <param name="cancellationToken">El token de cancelación.</param>
        /// <returns>Una tarea que representa la operación asíncrona con la lista de ConsumidorResponse.</returns>
        public Task<List<ConsumidorResponse>> Handle(ConsultarConsumidoresQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
                {
                    _logger.LogWarning("ConsultarPrestadoresQueryHandler.Handle: Request nulo.");
                    throw new RequestNullException("Request nulo en ConsultarConsumidoresQuery de ConsumidorController");
                }
                else
                {
                    return HandleAsync();
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Request nulo en ConsultarConsumidoresQuery de ConsumidorController");
                throw;
            }
            catch (Exception)
            {
                _logger.LogWarning("ConsultarPrestadoresQueryHandler.Handle: ArgumentNullException");
                throw;
            }
        }

        /// <summary>
        /// Maneja la operación asíncrona para recuperar una lista de ConsumidorResponse.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona con la lista de ConsumidorResponse.</returns>
        private async Task<List<ConsumidorResponse>> HandleAsync()
        {
            try
            {
                _logger.LogInformation("ConsultarUsuariosQueryHandler.HandleAsync");

                var result = await _dbContext.Consumidores.ToListAsync();

                var responseList = result.Select(entity => UsuarioMapper.MapEntityConsumidorAResponse(entity)).ToList();

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
