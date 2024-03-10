using MediatR;
using Microsoft.Extensions.Logging;

using UCABPagaloTodoMS.Application.Commands.FeatureCampo;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureCampo
{
    /// <summary>
    /// Clase que maneja el comando para agregar un consumidor.
    /// </summary>
    public class CrearCampoCommandHandler : IRequestHandler<CrearCampoCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<CrearCampoCommandHandler> _logger;

        public CrearCampoCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<CrearCampoCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para agregar un campo.
        /// </summary>
        /// <param name="request">Comando para agregar un campo.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del campo agregado.</returns>
        public async Task<Guid> Handle(CrearCampoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Campo == null)
                {
                    _logger.LogWarning("CrearCampoCommandHandler.Handle: Request nulo.");
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
        private async Task<Guid> HandleAsync(CrearCampoCommand request)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("CrearCampoCommandHandler.HandleAsync {Request}", request);
                var entity = CampoMapper.MapRequestCampoEntity(request.Campo);
                _dbContext.Campos.Add(entity);
                var id = entity.Id;
                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();
                _logger.LogInformation("CrearCampoCommandHandler.HandleAsync {Response}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error CrearCampoCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw;
            }
        }
    }
}
