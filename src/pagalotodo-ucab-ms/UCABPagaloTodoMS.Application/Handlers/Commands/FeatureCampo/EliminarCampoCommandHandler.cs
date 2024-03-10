using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureCampo;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureCampo
{
    /// <summary>
    /// Clase que maneja el comando para eliminar un campo.
    /// </summary>
    public class EliminarCampoCommandHandler : IRequestHandler<EliminarCampoCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<EliminarCampoCommandHandler> _logger;

        public EliminarCampoCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<EliminarCampoCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para eliminar un campo.
        /// </summary>
        /// <param name="command">Comando para aliminar un campo.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Mensaje de que el campo ha sido eliminado</returns>
        public async Task<Guid> Handle(EliminarCampoCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var campo = await _dbContext.Campos.FindAsync(command.Id);
                if (campo == null)
                {
                    _logger.LogWarning("EliminarCampoCommandHandler.Handle: No se encontró el campo con Id {Id}.", command.Id);
                    return default;
                }

                _dbContext.Campos.Remove(campo);
                await _dbContext.SaveEfContextChanges("APP");
                _logger.LogInformation("EliminarCampoCommandHandler.Handle: campo con Id {Id} eliminado exitosamente.", command.Id);
                return campo.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error EliminarCampoCommandHandler.Handle. {Mensaje}", ex.Message);
                throw;
            }
        }
    }
}
