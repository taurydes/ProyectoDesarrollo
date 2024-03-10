using UCABPagaloTodoMS.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Core.Entities;
using FluentAssertions.Common;
using UCABPagaloTodoMS.Application.Mappers;

namespace UCABPagaloTodoMS.Application.Handlers.Queries
{
    public class ConsultarServicioPrestadoQueryHandler : IRequestHandler<ConsultarServicioPrestadoQuery, List<ServicioPrestadoResponse>>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ConsultarServicioPrestadoQueryHandler> _logger;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConsultarServicioPrestadoPorIdQueryHandler"/>.
        /// </summary>
        /// <param name="dbContext">El contexto de la base de datos.</param>
        /// <param name="logger">El registrador.</param>
        public ConsultarServicioPrestadoQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarServicioPrestadoQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja la consulta de ConsultarServicioPrestadoPorIdQuery y recupera un ServicioPrestadoResponse.
        /// </summary>
        /// <param name="request">La consulta de ConsultarServicioPrestadoPorIdQuery.</param>
        /// <param name="cancellationToken">El token de cancelación.</param>
        /// <returns>Una tarea que representa la operación asíncrona con el ServicioPrestadoResponse.</returns>
        public Task<List<ServicioPrestadoResponse>> Handle(ConsultarServicioPrestadoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
                {
                    _logger.LogWarning("ConsultarPrestadoresQueryHandler.Handle: Request nulo.");
                    throw new ArgumentNullException(nameof(request));
                }
                else
                {
                    return HandleAsync();
                }
            }
            catch (Exception)
            {
                _logger.LogWarning("ConsultarPrestadoresQueryHandler.Handle: ArgumentNullException");
                throw;
            }
        }

        /// <summary>
        /// Maneja la operación asíncrona para recuperar un ServicioPrestadoResponse.
        /// </summary>
        /// <param name="id">El ID del servicio prestado.</param>
        /// <returns>Una tarea que representa la operación asíncrona con el ServicioPrestadoResponse.</returns>
        private async Task<List<ServicioPrestadoResponse>> HandleAsync()
        {
            try
            {
                _logger.LogInformation("ConsultarUsuariosQueryHandler.HandleAsync");

                var result = await _dbContext.ServiciosPrestados.ToListAsync();

                var responseList = result.Select(entity => ServicioPrestadoMapper.MapEntityAResponse(entity)).ToList();

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
