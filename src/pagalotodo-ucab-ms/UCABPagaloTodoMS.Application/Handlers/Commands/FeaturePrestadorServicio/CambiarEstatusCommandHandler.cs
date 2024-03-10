using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeaturePrestadorServicio;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeaturePrestadorServicio
{
    /// <summary>
    /// Clase que maneja el comando para cambiar el estatus de un prestador de servicio.
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
        /// Maneja el comando para cambiar el estatus de un prestador de servicio.
        /// </summary>
        /// <param name="request">Comando para cambiar el estatus de un prestador de servicio.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del prestador de servicio cuyo estatus ha sido cambiado.</returns>
        public async Task<Guid> Handle(CambiarEstatusCommand request, CancellationToken cancellationToken)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("CambiarEstatusCommandHandler.HandleAsync {Request}", request);
                var entity = await _dbContext.PrestadorServicios.FindAsync(request.Id);
                if (entity == null)
                {
                    _logger.LogWarning("CambiarEstatusCommandHandler.HandleAsync: Usuario no encontrado.");
                    return default;
                }
                //actualizar datos globales Usuario
                if (entity.EstatusCuenta == true)
                {
                entity.EstatusCuenta = false;
                }
                else if (entity.EstatusCuenta == false)
                {
                    entity.EstatusCuenta = true;
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
