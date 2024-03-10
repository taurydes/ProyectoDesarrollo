using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;

public class ConsultarAdministradoresPorIdQueryHandler : IRequestHandler<ConsultarAdministradoresPorIdQuery, AdministradorResponse>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<ConsultarAdministradoresPorIdQueryHandler> _logger;

    /// <summary>
    /// Constructor de la clase ConsultarAdministradoresPorIdQueryHandler.
    /// </summary>
    /// <param name="dbContext">Contexto de la base de datos.</param>
    /// <param name="logger">Instancia del logger.</param>
    public ConsultarAdministradoresPorIdQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarAdministradoresPorIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la consulta para obtener un administrador por su ID.
    /// </summary>
    /// <param name="request">Consulta para obtener un administrador.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Respuesta con los datos del administrador consultado.</returns>
    public async Task<AdministradorResponse> Handle(ConsultarAdministradoresPorIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                _logger.LogWarning("ConsultarUsuarioPorIdQueryHandler.Handle: Request nulo.");
                throw new RequestNullException("El objeto request no puede ser nulo.");
            }
            else
            {
                return await HandleAsync(request.Id);
            }
        }
        catch (Exception)
        {
            _logger.LogWarning("ConsultarUsuarioPorIdQueryHandler.Handle: ArgumentNullException");
            throw;
        }
    }

    /// <summary>
    /// Maneja la consulta asíncrona para obtener un administrador por su ID.
    /// </summary>
    /// <param name="id">ID del administrador a consultar.</param>
    /// <returns>Respuesta con los datos del administrador consultado.</returns>
    private async Task<AdministradorResponse> HandleAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("ConsultarUsuarioPorIdQueryHandler.HandleAsync");

            var usuario = await _dbContext.Administradores.FindAsync(id);

            if (usuario is null)
            {
                throw new AdministradorNotFoundException($"No se encontró ningún administrador con el ID: {id}");
            }

            var response = UsuarioMapper.MapEntityAdministradorAResponse(usuario);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ConsultarUsuarioPorIdQueryHandler.HandleAsync. {Mensaje}", ex.Message);
            throw;
        }
    }

}
