using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureServiciosPrestados;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureServiciosPrestados
{
    /// <summary>
    /// Clase que maneja el comando para cambiar el estatus de un servicio prestado.
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
        /// Maneja el comando para cambiar el estatus de un servicio prestado.
        /// </summary>
        /// <param name="request">Comando para cambiar el estatus de un servicio prestado.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del servicio prestado cuyo estatus ha sido cambiado.</returns>
        public async Task<Guid> Handle(CambiarEstatusCommand request, CancellationToken cancellationToken)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("CambiarEstatusCommandHandler.HandleAsync {Request}", request);
                var entity = await _dbContext.ServiciosPrestados.FindAsync(request.Id);
                if (entity == null)
                {
                    _logger.LogWarning("CambiarEstatusCommandHandler.HandleAsync: Pago no encontrado.");
                    return default;
                }
                //actualizar datos globales Usuario
                if (entity.EstatusServicio == true)
                {
                entity.EstatusServicio = false;
                }
                else if (entity.EstatusServicio == false)
                {
                    entity.EstatusServicio = true;
                }

                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();

                _logger.LogInformation("CambiarEstatusCommandHandler.HandleAsync {Response}", entity.Id);
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CambiarEstatusCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw;
            }
        }

    }
}
