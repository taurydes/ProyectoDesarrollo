using MediatR;
using Microsoft.Extensions.Logging;

using UCABPagaloTodoMS.Application.Commands.FeatureCampoPago;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureCampoPago
{
    /// <summary>
    /// Clase que maneja el comando para agregar un campo de pago.
    /// </summary>
    public class CrearCampoPagoCommandHandler : IRequestHandler<CrearCampoPagoCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<CrearCampoPagoCommandHandler> _logger;

        public CrearCampoPagoCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<CrearCampoPagoCommandHandler> logger)
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
        public async Task<Guid> Handle(CrearCampoPagoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.CampoPago == null)
                {
                    _logger.LogWarning("CrearCampoPagoCommandHandler.Handle: Request nulo.");
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
        private async Task<Guid> HandleAsync(CrearCampoPagoCommand request)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("CrearCampoPagoCommandHandler.HandleAsync {Request}", request);
                var entity = CampoPagoMapper.MapRequestCampoEntity(request.CampoPago);
                _dbContext.CamposPago.Add(entity);
                var id = entity.Id;
                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();
                _logger.LogInformation("CrearCampoPagoCommandHandler.HandleAsync {Response}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error CrearCampoPagoCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw;
            }
        }
    }
}
