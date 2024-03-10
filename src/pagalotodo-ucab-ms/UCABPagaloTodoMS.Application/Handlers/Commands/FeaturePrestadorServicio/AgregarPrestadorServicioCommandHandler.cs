using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureConsumidor;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeaturePrestadorServicio
{
    /// <summary>
    /// Clase que maneja el comando para agregar un prestador de servicio.
    /// </summary>
    public class AgregarPrestadorServicioCommandHandler : IRequestHandler<AgregarPrestadorServicioCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<AgregarPrestadorServicioCommandHandler> _logger;

        public AgregarPrestadorServicioCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<AgregarPrestadorServicioCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para agregar un prestador de servicio.
        /// </summary>
        /// <param name="request">Comando para agregar un prestador de servicio.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del prestador de servicio agregado.</returns>
        public async Task<Guid> Handle(AgregarPrestadorServicioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Prestador == null)
                {
                    _logger.LogWarning("AgregarUsuarioCommandHandler.Handle: Request nulo.");
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

        private async Task<Guid> HandleAsync(AgregarPrestadorServicioCommand request)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("AgregarConsumidorCommandHandler.HandleAsync {Request}", request);
                var entity = UsuarioMapper.MapRequestPrestadorServicioEntity(request.Prestador);
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
