using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Database;

public class ConsultarConsumidoresPorIdQueryHandler : IRequestHandler<ConsultarConsumidoresPorIdQuery, ConsumidorResponse>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<ConsultarConsumidoresPorIdQueryHandler> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ConsultarConsumidoresPorIdQueryHandler"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de la base de datos.</param>
    /// <param name="logger">El registrador.</param>
    public ConsultarConsumidoresPorIdQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarConsumidoresPorIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la consulta de ConsultarConsumidoresPorIdQuery y recupera un ConsumidorResponse.
    /// </summary>
    /// <param name="request">La consulta de ConsultarConsumidoresPorIdQuery.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Una tarea que representa la operación asíncrona con el ConsumidorResponse.</returns>
    public async Task<ConsumidorResponse> Handle(ConsultarConsumidoresPorIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                _logger.LogWarning("ConsultarConsumidoresPorIdQueryHandler.Handle: Request nulo.");
                throw new ArgumentNullException(nameof(request));
            }
            else
            {
                return await HandleAsync(request.Id);
            }
        }
        catch (Exception)
        {
            _logger.LogWarning("ConsultarConsumidoresPorIdQueryHandler.Handle: ArgumentNullException");
            throw;
        }
    }

    /// <summary>
    /// Maneja la operación asíncrona para recuperar un ConsumidorResponse por su ID.
    /// </summary>
    /// <param name="id">El ID del consumidor.</param>
    /// <returns>Una tarea que representa la operación asíncrona con el ConsumidorResponse.</returns>
    private async Task<ConsumidorResponse> HandleAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("ConsultarConsumidoresPorIdQueryHandler.HandleAsync");

            var consumidor = await _dbContext.Consumidores
                .Include(p => p.PagosRealizados)
                .ThenInclude(p=>p.CamposPago)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (consumidor is null)
            {
                throw new ConsumidorNotFoundException(id);
            }

            var response = UsuarioMapper.MapEntityConsumidorAResponse(consumidor);

            return response;
        }
        catch (ConsumidorNotFoundException ex)
        {
            _logger.LogError(ex, $"Error al consultar consumidores. {ex.Message}");
            throw; // Lanza la excepción personalizada hacia arriba para ser manejada en un nivel superior.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ConsultarUsuarioPorIdQueryHandler.HandleAsync. {Mensaje}", ex.Message);
            throw;
        }
    }
}