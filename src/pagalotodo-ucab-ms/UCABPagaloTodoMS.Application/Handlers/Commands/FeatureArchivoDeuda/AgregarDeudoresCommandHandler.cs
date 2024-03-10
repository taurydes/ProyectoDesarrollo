using MediatR;
using Microsoft.Extensions.Logging;

using UCABPagaloTodoMS.Application.Commands.FeatureArchivoDeuda;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureCampo
{
    /// <summary>
    /// Clase que maneja el comando para agregar un deudor.
    /// </summary>
    public class AgregarDeudoresCommandHandler : IRequestHandler<AgregarDeudoresCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<AgregarDeudoresCommandHandler> _logger;

        public AgregarDeudoresCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<AgregarDeudoresCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para agregar un deudor.
        /// </summary>
        /// <param name="request">Comando para agregar un deudor.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del deudor agregado.</returns>
        public async Task<Guid> Handle(AgregarDeudoresCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Deudor == null)
                {
                    _logger.LogWarning("AgregarDeudoresCommandHandler.Handle: Request nulo.");
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
        /// Maneja el comando para agregar un campo de forma asíncrona.
        /// </summary>
        /// <param name="request">Comando para agregar un campo.</param>
        /// <returns>Identificador del campo agregado.</returns>
        private async Task<Guid> HandleAsync(AgregarDeudoresCommand request)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("AgregarDeudoresCommandHandler.HandleAsync {Request}", request);
                var entity = ArchivoDeudaMapper.MapRequestDeudorEntity(request.Deudor);
                _dbContext.ArchivoDeudas.Add(entity);
                var id = entity.Id;
                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();
                _logger.LogInformation("AgregarDeudoresCommandHandler.HandleAsync {Response}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error AgregarDeudoresCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw;
            }
        }
    }
}
