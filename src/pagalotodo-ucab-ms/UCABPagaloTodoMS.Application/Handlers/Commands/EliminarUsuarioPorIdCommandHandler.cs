using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers;

[ExcludeFromCodeCoverage]
public class EliminarUsuarioPorIdCommandHandler : IRequestHandler<EliminarUsuarioCommand, Guid>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<EliminarUsuarioPorIdCommandHandler> _logger;

    /// <summary>
    /// Constructor de la clase EliminarUsuarioPorIdCommandHandler.
    /// </summary>
    /// <param name="dbContext">Contexto de la base de datos.</param>
    /// <param name="logger">Instancia del logger.</param>
    public EliminarUsuarioPorIdCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<EliminarUsuarioPorIdCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    /// <summary>
    /// Maneja el comando para eliminar un usuario por su ID.
    /// </summary>
    /// <param name="command">Comando para eliminar un usuario.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Identificador del usuario eliminado.</returns>
    public async Task<Guid> Handle(EliminarUsuarioCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var usuario = await _dbContext.Usuarios.FindAsync(command.Id);
            if (usuario == null)
            {
                _logger.LogWarning("EliminarUsuarioPorIdCommandHandler.Handle: No se encontró el usuario con Id {Id}.", command.Id);
                return default;
            }

            _dbContext.Usuarios.Remove(usuario);
            await _dbContext.SaveEfContextChanges("APP");
            _logger.LogInformation("EliminarUsuarioPorIdCommandHandler.Handle: Usuario con Id {Id} eliminado exitosamente.", command.Id);
            return usuario.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error EliminarUsuarioPorIdCommandHandler.Handle. {Mensaje}", ex.Message);
            throw;
        }
    }
}
