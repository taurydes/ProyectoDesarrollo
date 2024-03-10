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
    /// Clase que maneja el comando para Agregar una configuracion de pago a un servicio prestado.
    /// </summary>
    public class AgregaConfiguracionCommandHandler : IRequestHandler<AgregaConfiguracionCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<AgregaConfiguracionCommandHandler> _logger;

        public AgregaConfiguracionCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<AgregaConfiguracionCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para agregar una configuracion de pago a un servicio prestado.
        /// </summary>
        /// <param name="request">Comando para agregar una configuracion de pago a un servicio prestado.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>servicio prestado cuya configuracion de pago ha sido agregada.</returns>
        public async Task<Guid> Handle(AgregaConfiguracionCommand request, CancellationToken cancellationToken)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("AgregaConfiguracionCommandHandler.HandleAsync {Request}", request);
                var entity = await _dbContext.ServiciosPrestados.FindAsync(request.ServicioId);
                if (entity == null)
                {
                    _logger.LogWarning("AgregaConfiguracionCommandHandler.HandleAsync: Pago no encontrado.");
                    return default;

                }
                //actualizar datos de la configuración Usuario
                
                entity.ConfiguracionPagoId = request.Configuracionid;
               
                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();

                _logger.LogInformation("AgregaConfiguracionCommandHandler.HandleAsync {Response}", entity.Id);
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en AgregaConfiguracionCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw;
            }
        }

    }
}
