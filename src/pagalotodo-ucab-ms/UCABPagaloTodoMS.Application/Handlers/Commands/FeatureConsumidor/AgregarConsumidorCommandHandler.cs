using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureConsumidor;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureConsumidor
{
    /// <summary>
    /// Clase que maneja el comando para agregar un consumidor.
    /// </summary>
    public class AgregarConsumidorCommandHandler : IRequestHandler<AgregarConsumidorCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<AgregarConsumidorCommandHandler> _logger;

        public AgregarConsumidorCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<AgregarConsumidorCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para agregar un consumidor.
        /// </summary>
        /// <param name="request">Comando para agregar un consumidor.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del consumidor agregado.</returns>
        public async Task<Guid> Handle(AgregarConsumidorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request._request == null)
                {
                    _logger.LogWarning("AgregarConsumidorCommandHandler.Handle: Request nulo.");
                    throw new ArgumentNullException(nameof(request));
                }
                else
                {
                    return await HandleAsync(request);
                }
            }
            catch (Exception)
            {
                throw new RequestNullException("El objeto request no puede ser nulo.");
            }
        }

        /// <summary>
        /// Maneja el comando para agregar un consumidor de forma asíncrona.
        /// </summary>
        /// <param name="request">Comando para agregar un consumidor.</param>
        /// <returns>Identificador del consumidor agregado.</returns>
        private async Task<Guid> HandleAsync(AgregarConsumidorCommand request)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("AgregarConsumidorCommandHandler.HandleAsync {Request}", request);
                var entity = UsuarioMapper.MapRequestConsumidorEntity(request._request);
                _dbContext.Usuarios.Add(entity);
                var id = entity.Id;
                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();
                _logger.LogInformation("AgregarConsumidorCommandHandler.HandleAsync {Response}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error AgregarConsumidorCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw;
            }
        }
    }
}
