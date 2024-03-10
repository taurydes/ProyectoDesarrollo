using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureServiciosPrestados;
using UCABPagaloTodoMS.Core.Database;

public class EliminarServicioPrestadoPorIdCommandHandler : IRequestHandler<EliminarServicioPrestadoCommand, Guid>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<EliminarServicioPrestadoPorIdCommandHandler> _logger;

    /// <summary>
    /// Constructor de la clase EliminarServicioPrestadoPorIdCommandHandler.
    /// </summary>
    /// <param name="dbContext">Contexto de la base de datos.</param>
    /// <param name="logger">Instancia del logger.</param>
    public EliminarServicioPrestadoPorIdCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<EliminarServicioPrestadoPorIdCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Maneja el comando para eliminar un servicio prestado por su ID.
    /// </summary>
    /// <param name="command">Comando para eliminar un servicio prestado.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Identificador del servicio prestado eliminado.</returns>
    public async Task<Guid> Handle(EliminarServicioPrestadoCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var servicio = await _dbContext.ServiciosPrestados.FindAsync(command.Id);
            if (servicio == null)
            {
                _logger.LogWarning("EliminarUsuarioPorIdCommandHandler.Handle: No se encontró el usuario con Id {Id}.", command.Id);
                return default;
            }

            _dbContext.ServiciosPrestados.Remove(servicio);
            await _dbContext.SaveEfContextChanges("APP");
            _logger.LogInformation("EliminarUsuarioPorIdCommandHandler.Handle: Usuario con Id {Id} eliminado exitosamente.", command.Id);
            return servicio.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error EliminarUsuarioPorIdCommandHandler.Handle. {Mensaje}", ex.Message);
            throw;
        }
    }
}
