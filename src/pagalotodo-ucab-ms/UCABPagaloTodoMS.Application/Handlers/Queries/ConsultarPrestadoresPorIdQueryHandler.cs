using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;

public class ConsultarPrestadoresPorIdQueryHandler : IRequestHandler<ConsultarPrestadoresPorIdQuery, PrestadorServicioResponse>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<ConsultarPrestadoresPorIdQueryHandler> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ConsultarPrestadoresPorIdQueryHandler"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de la base de datos.</param>
    /// <param name="logger">El registrador.</param>
    public ConsultarPrestadoresPorIdQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarPrestadoresPorIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la consulta de ConsultarPrestadoresPorIdQuery y recupera un PrestadorServicioResponse.
    /// </summary>
    /// <param name="request">La consulta de ConsultarPrestadoresPorIdQuery.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Una tarea que representa la operación asíncrona con el PrestadorServicioResponse.</returns>
    public async Task<PrestadorServicioResponse> Handle(ConsultarPrestadoresPorIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                _logger.LogWarning("ConsultarUsuarioPorIdQueryHandler.Handle: Request nulo.");
                throw new RequestNullException("El Request es nulo");
            }
            else
            {
                return await HandleAsync(request.Id);
            }
        }
        catch (RequestNullException ex)
        {
            _logger.LogError(ex, "Request nulo.");
            throw; // Lanza la excepción personalizada hacia arriba para ser manejada en un nivel superior.
        }
        catch (Exception)
        {
            _logger.LogWarning("ConsultarUsuarioPorIdQueryHandler.Handle: ArgumentNullException");
            throw;
        }
    }

    /// <summary>
    /// Maneja la operación asíncrona para recuperar un PrestadorServicioResponse por su ID.
    /// </summary>
    /// <param name="id">El ID del prestador de servicios.</param>
    /// <returns>Una tarea que representa la operación asíncrona con el PrestadorServicioResponse.</returns>
    private async Task<PrestadorServicioResponse> HandleAsync(Guid id)
{
    try
    {
        _logger.LogInformation("ConsultarUsuarioPorIdQueryHandler.HandleAsync");

        var prestadorServicio = await _dbContext.PrestadorServicios
            .Include(p => p.ServiciosPrestados)
            .ThenInclude(sp => sp.PagosPorServicioRealizados)
            .FirstOrDefaultAsync(p => p.Id == id);

            if (prestadorServicio is null)
            {
                throw new PrestadorServicioNotFoundException();
            }

            var response = UsuarioMapper.MapEntityPrestadorServicioAResponse(prestadorServicio);

            return response;
    }
        catch (PrestadorServicioNotFoundException ex)
        {
            _logger.LogError(ex, "Error ConsultarUsuarioPorIdQueryHandler.HandleAsync. {Mensaje}", ex.Message);
            throw;
        }
        catch (Exception ex)
    {
        _logger.LogError(ex, "Error ConsultarUsuarioPorIdQueryHandler.HandleAsync. {Mensaje}", ex.Message);
        throw;
    }
    }
}