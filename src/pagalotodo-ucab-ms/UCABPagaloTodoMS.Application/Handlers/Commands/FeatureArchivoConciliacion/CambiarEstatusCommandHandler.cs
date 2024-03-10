using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Commands.FeatureArchivoConciliacion;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureArchivoConciliacion
{
    /// <summary>
    /// Clase que maneja el comando para cambiar el estatus de un consumidor.
    /// </summary>
    public class CambiarEstatusCommandHandler : IRequestHandler<CambiarEstatusCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<CambiarEstatusCommandHandler> _logger;

        public CambiarEstatusCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<CambiarEstatusCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para cambiar el estatus de un consumidor.
        /// </summary>
        /// <param name="request">Comando para cambiar el estatus de un consumidor.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del consumidor cuyo estatus ha sido cambiado.</returns>
        public async Task<Guid> Handle(CambiarEstatusCommand request, CancellationToken cancellationToken)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("CambiarEstatusCommandHandler.HandleAsync {Request}", request);
                var entity = await _dbContext.ArchivoConciliacions.FindAsync(request.Id);
                if (entity == null)
                {
                    _logger.LogWarning("CambiarEstatusCommandHandler.HandleAsync: Usuario no encontrado.");
                    return default;
                }
                //actualizar datos globales Usuario
                if (entity.Confirmado == true)
                {
                entity.Confirmado = false;
                }
                else if (entity.Confirmado == false)
                {
                    entity.Confirmado = true;
                }

                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();

                _logger.LogInformation("CambiarEstatusCommandHandler.HandleAsync {Response}", entity.Id);
                return entity.Id;
            }
            catch (UsuarioNotFoundException ex)
            {
                _logger.LogError(ex, "Ocurrió un error en ActualizarContrasena. {Mensaje}", ex.Message);
                throw new UsuarioNotFoundException("Usuario no encontrado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CambiarEstatusCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw new UsuarioNotFoundException("Usuario no encontrado.");
            }
        }

    }
}
