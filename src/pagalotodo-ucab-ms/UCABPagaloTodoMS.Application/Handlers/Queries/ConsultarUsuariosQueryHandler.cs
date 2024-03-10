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
    public class ConsultarUsuariosQueryHandler : IRequestHandler<ConsultarUsuariosQuery, List<UsuarioResponse>>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ConsultarUsuariosQueryHandler> _logger;

        /// <summary>
        /// Inicializa una nueva instancia de la clase ConsultarUsuariosQueryHandler.
        /// </summary>
        /// <param name="dbContext">El contexto de la base de datos.</param>
        /// <param name="logger">El registrador.</param>
        public ConsultarUsuariosQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarUsuariosQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja la consulta ConsultarUsuariosQuery y devuelve la lista de usuarios.
        /// </summary>
        /// <param name="request">La consulta ConsultarUsuariosQuery.</param>
        /// <param name="cancellationToken">El token de cancelación.</param>
        /// <returns>Una lista de objetos UsuarioResponse que contienen la información de los usuarios.</returns>
        public Task<List<UsuarioResponse>> Handle(ConsultarUsuariosQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
                {
                    _logger.LogWarning("ConsultarUsuariosQueryHandler.Handle: Request nulo.");
                    throw new RequestNullException("El objeto request no puede ser nulo.");
                }
                else
                {
                    return HandleAsync();
                }
            }
            catch (Exception)
            {
                _logger.LogWarning("ConsultarUsuariosQueryHandler.Handle: ArgumentNullException");
                throw;
            }
        }

        /// <summary>
        /// Recupera la lista de usuarios desde la base de datos.
        /// </summary>
        /// <returns>Una lista de objetos UsuarioResponse que contienen la información de los usuarios.</returns>
        private async Task<List<UsuarioResponse>> HandleAsync()
        {
            try
            {
                _logger.LogInformation("ConsultarUsuariosQueryHandler.HandleAsync");

                var result = await _dbContext.Usuarios.ToListAsync();

                var responseList = result.Select(entity => UsuarioMapper.MapEntityAResponse(entity)).ToList();

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
