using Azure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Handlers.Queries;

public class ConsultarServicioPrestadoPorIdQueryHandler : IRequestHandler<ConsultarServicioPrestadoPorIdQuery, ServicioPrestadoResponse>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<ConsultarServicioPrestadoPorIdQueryHandler> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ConsultarServicioPrestadoPorIdQueryHandler"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de la base de datos.</param>
    /// <param name="logger">El registrador.</param>
    public ConsultarServicioPrestadoPorIdQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarServicioPrestadoPorIdQueryHandler> logger)
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
    public async Task<ServicioPrestadoResponse> Handle(ConsultarServicioPrestadoPorIdQuery request, CancellationToken cancellationToken)
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
            _logger.LogWarning("El Request es nulo");
            throw;
        }
        catch (Exception)
        {
            _logger.LogWarning("ConsultarUsuarioPorIdQueryHandler.Handle: ArgumentNullException");
            throw;
        }
    }

    /// <summary>
    /// Maneja la operación asíncrona para recuperar un ServicioPrestadoResponse.
    /// </summary>
    /// <param name="id">El ID del servicio prestado.</param>
    /// <returns>Una tarea que representa la operación asíncrona con el ServicioPrestadoResponse.</returns>
    private async Task<ServicioPrestadoResponse> HandleAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("ConsultarUsuarioPorIdQueryHandler.HandleAsync");

            var servicio = await _dbContext.ServiciosPrestados
            .Include(p => p.PagosPorServicioRealizados).ThenInclude(p => p.CamposPago)
            .Include(p => p.ArchivosPertenecientes) // Incluir la lista de archivos de conciliación
            .FirstOrDefaultAsync(p => p.Id == id);

            if (servicio is null)
            {
                throw new ServicioPrestadoNotFoundException($"No se encontró el servicio prestado con el ID: {id}");
            }

            var response = ServicioPrestadoMapper.MapEntityAResponse(servicio);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ConsultarUsuarioPorIdQueryHandler.HandleAsync. {Mensaje}", ex.Message);
            throw;
        }
    }
}