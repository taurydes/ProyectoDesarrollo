using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureArchivoConciliacion
{
    /// <summary>
    /// Manejador de comando para agregar un Archivo.
    /// </summary>
    public class AgregarArchivoConciliacionCommandHandler : IRequestHandler<AgregarArchivoConciliacionCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<AgregarArchivoConciliacionCommandHandler> _logger;


        public AgregarArchivoConciliacionCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<AgregarArchivoConciliacionCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para agregar un archivo.
        /// </summary>
        /// <param name="request">Comando para agregar un archivo.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del archivo agregado.</returns>
        public async Task<Guid> Handle(AgregarArchivoConciliacionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ArchivoRequest == null)
                {
                    _logger.LogWarning("AgregarArchivoConciliacionCommandHandler.Handle: Request nulo.");
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
        /// Maneja el comando para agregar un administrador, sin embargo sin Token de Cancelación por una Excepción o Fallo
        /// </summary>
        /// <param name="request">Comando para agregar un administrador.</param>
        /// <returns>Administrador agregado.</returns>
        private async Task<Guid> HandleAsync(AgregarArchivoConciliacionCommand request)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("AgregarArchivoConciliacionCommandHandler.HandleAsync {Request}", request);
                var entity = ArchivoConciliacionMapper.MapRequestConciliacionEntity(request.ArchivoRequest);
                _dbContext.ArchivoConciliacions.Add(entity);
                var id = entity.Id;
                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();
                _logger.LogInformation("AgregarArchivoConciliacionCommandHandler.HandleAsync {Response}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error AgregarArchivoConciliacionCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw;
            }
        }
    }
}
