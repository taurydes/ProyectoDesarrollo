using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Commands.FeatureCampo;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureCampo
{  
    /// <summary>
   /// Clase que maneja el comando para actualizar un campo.
   /// </summary>
    public class ActualizarCampoCommandHandler : IRequestHandler<ActualizarCampoCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ActualizarCampoCommandHandler> _logger;

        public ActualizarCampoCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ActualizarCampoCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para actualizar un campo.
        /// </summary>
        /// <param name="request">Comando para actualizar un campo.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del campo actualizado.</returns>
        public async Task<Guid> Handle(ActualizarCampoCommand request, CancellationToken cancellationToken)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("ActualizarCampoCommand.HandleAsync {Request}", request);
                var entity = await _dbContext.Campos.FindAsync(request.Id);
                if (entity == null)
                {
                    _logger.LogWarning("ActualizarCampoCommand.HandleAsync: Campo no encontrado.");
                    return default;
                }
                //actualizar datos globales Usuario
                CampoMapper.MapEntityUpdateResponse(entity,request);
                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();

                _logger.LogInformation("ActualizarCampoCommand.HandleAsync {Response}", entity.Id);
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ActualizarCampoCommand.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                 throw new Exception($"No se encontró el Campo con ID {request.Id} en la base de datos.", ex);
            }
        }

    }
}
