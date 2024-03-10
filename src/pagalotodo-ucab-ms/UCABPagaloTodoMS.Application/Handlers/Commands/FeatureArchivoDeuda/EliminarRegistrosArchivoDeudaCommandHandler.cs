using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Core.Database;

public class EliminarRegistrosArchivoDeudaCommandHandler : IRequestHandler<EliminarRegistrosArchivoDeudaCommand, Guid>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<EliminarRegistrosArchivoDeudaCommandHandler> _logger;

    /// <summary>
    /// Constructor de la clase EliminarRegistrosArchivoDeudaCommandHandler.
    /// </summary>
    /// <param name="dbContext">Contexto de la base de datos.</param>
    /// <param name="logger">Instancia del logger.</param>
    public EliminarRegistrosArchivoDeudaCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<EliminarRegistrosArchivoDeudaCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    /// <summary>
    /// Maneja el comando para eliminar un usuario por su ID.
    /// </summary>
    /// <param name="request">Comando para eliminar un usuario.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Identificador del usuario eliminado.</returns>
    public async Task<Guid> Handle(EliminarRegistrosArchivoDeudaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var consultas = await _dbContext.ArchivoDeudas.Where(a => a.ServicioPrestadoId == request.ServicioId).ToListAsync();
            if (consultas == null || !consultas.Any())
            {
                _logger.LogWarning("EliminarRegistrosArchivoDeudaCommandHandler.Handle: No se encontraron registros con el ServicioId {ServicioId}.", request.ServicioId);
                return default;
            }

            foreach (var consulta in consultas)
            {
                _dbContext.ArchivoDeudas.Remove(consulta);
            }
            await _dbContext.SaveEfContextChanges("APP");
            _logger.LogInformation("EliminarRegistrosArchivoDeudaCommandHandler.Handle: Registros con ServicioId {ServicioId} eliminados exitosamente.", request.ServicioId);
            return consultas.FirstOrDefault()?.Id ?? default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error EliminarRegistrosArchivoDeudaCommandHandler.Handle. {Mensaje}", ex.Message);
            throw;
        }
    }
}
