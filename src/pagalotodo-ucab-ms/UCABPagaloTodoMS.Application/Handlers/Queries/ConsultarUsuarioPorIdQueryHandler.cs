using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Database;

public class ConsultarUsuarioPorIdQueryHandler : IRequestHandler<ConsultarUsuarioPorIdQuery, UsuarioResponse>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<ConsultarUsuarioPorIdQueryHandler> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de la clase ConsultarUsuarioPorIdQueryHandler.
    /// </summary>
    /// <param name="dbContext">El contexto de la base de datos.</param>
    /// <param name="logger">El registrador.</param>
    public ConsultarUsuarioPorIdQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarUsuarioPorIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la consulta ConsultarUsuarioPorIdQuery y devuelve la información de un usuario según su ID.
    /// </summary>
    /// <param name="request">La consulta ConsultarUsuarioPorIdQuery.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Un objeto UsuarioResponse que contiene la información del usuario.</returns>
    public async Task<UsuarioResponse> Handle(ConsultarUsuarioPorIdQuery request, CancellationToken cancellationToken)
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
    /// Recupera la información de un usuario según su ID desde la base de datos.
    /// </summary>
    /// <param name="id">El ID del usuario.</param>
    /// <returns>Un objeto UsuarioResponse que contiene la información del usuario.</returns>
    private async Task<UsuarioResponse> HandleAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("ConsultarUsuarioPorIdQueryHandler.HandleAsync");

            var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario is null)
            {
                throw new UsuarioNotFoundException($"No se encontró ningún usuario con el ID: {id}");
            }

            var response = UsuarioMapper.MapEntityAResponse(usuario);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ConsultarUsuarioPorIdQueryHandler.HandleAsync. {Mensaje}", ex.Message);
            throw;
        }
    }
}