using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureConfiguracionPago;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureConfiguracionPago
{
    /// <summary>
    /// Clase que maneja el comando para agregar una configuracion de pago.
    /// </summary>
    public class CrearConfiguracionPagoCommandHandler : IRequestHandler<CrearConfiguracionPagoCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<CrearConfiguracionPagoCommandHandler> _logger;

        public CrearConfiguracionPagoCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<CrearConfiguracionPagoCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para agregar una configuracion de pago.
        /// </summary>
        /// <param name="request">Comando para agregar una configuracion de pago.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador de la configuracion de pago agregado.</returns>
        public async Task<Guid> Handle(CrearConfiguracionPagoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ConfiguracionPago == null)
                {
                    _logger.LogWarning("CrearServicioPrestadoCommandHandler.Handle: Request nulo.");
                    throw new ArgumentNullException(nameof(request));
                }
                else
                {
                    return await HandleAsync(request);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Maneja el comando para agregar una configuracion de forma asíncrona.
        /// </summary>
        /// <param name="request">Comando para agregar una configuracion.</param>
        /// <returns>Identificador de la configuracion agregado.</returns>
        private async Task<Guid> HandleAsync(CrearConfiguracionPagoCommand request)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("CrearServicioPrestadoCommandHandler.HandleAsync {Request}", request);
                var entity = ConfiguracionPagoMapper.MapRequestPagoEntity(request.ConfiguracionPago);
                _dbContext.ConfiguracionPagos.Add(entity);
                var id = entity.Id;
                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();
                _logger.LogInformation("CrearServicioPrestadoCommandHandler.HandleAsync {Response}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error CrearServicioPrestadoCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw;
            }
        }
    }
}
