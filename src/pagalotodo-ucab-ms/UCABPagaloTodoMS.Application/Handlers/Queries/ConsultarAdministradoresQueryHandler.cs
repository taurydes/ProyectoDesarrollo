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
    public class ConsultarAdministradoresQueryHandler : IRequestHandler<ConsultarAdministradoresQuery, List<AdministradorResponse>>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ConsultarAdministradoresQueryHandler> _logger;

        /// <summary>
        /// Constructor de la clase ConsultarAdministradoresQueryHandler.
        /// </summary>
        /// <param name="dbContext">Contexto de la base de datos.</param>
        /// <param name="logger">Instancia del logger.</param>
        public ConsultarAdministradoresQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarAdministradoresQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja la consulta para obtener todos los administradores.
        /// </summary>
        /// <param name="request">Consulta para obtener los administradores.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista de respuestas con los datos de los administradores consultados.</returns>
        public Task<List<AdministradorResponse>> Handle(ConsultarAdministradoresQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
                {
                    _logger.LogWarning("ConsultarAdministradoresQueryHandler.Handle: Request nulo.");
                    throw new RequestNullException("El objeto request no puede ser nulo.");
                }
                else
                {
                    return HandleAsync();
                }
            }
            catch (Exception)
            {
                _logger.LogWarning("ConsultarAdministradoresQueryHandler.Handle: ArgumentNullException");
                throw;
            }
        }

        /// <summary>
        /// Maneja la consulta asíncrona para obtener todos los administradores.
        /// </summary>
        /// <returns>Lista de respuestas con los datos de los administradores consultados.</returns>
        private async Task<List<AdministradorResponse>> HandleAsync()
        {
            try
            {
                _logger.LogInformation("ConsultarAdministradoresQueryHandler.HandleAsync");

                var result = await _dbContext.Administradores.ToListAsync();

                var responseList = result.Select(entity => UsuarioMapper.MapEntityAdministradorAResponse(entity)).ToList();

                return responseList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ConsultarAdministradoresQueryHandler.HandleAsync. {Mensaje}", ex.Message);
                throw;
            }
        }

    }
}
