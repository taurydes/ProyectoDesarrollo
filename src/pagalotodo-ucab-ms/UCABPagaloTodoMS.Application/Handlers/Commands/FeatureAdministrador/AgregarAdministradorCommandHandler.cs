using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureConsumidor;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureAdministrador
{
    /// <summary>
    /// Manejador de comando para agregar un administrador.
    /// </summary>
    public class AgregarAdministradorCommandHandler : IRequestHandler<AgregarAdministradorCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<AgregarAdministradorCommandHandler> _logger;


        public AgregarAdministradorCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<AgregarAdministradorCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para agregar un administrador.
        /// </summary>
        /// <param name="request">Comando para agregar un administrador.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del administrador agregado.</returns>
        public async Task<Guid> Handle(AgregarAdministradorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Administrador == null)
                {
                    _logger.LogWarning("AgregarAdministradorCommandHandler.Handle: Request nulo.");
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
        private async Task<Guid> HandleAsync(AgregarAdministradorCommand request)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("AgregarAdministradorCommandHandler.HandleAsync {Request}", request);
                var entity = UsuarioMapper.MapRequestAdministradorEntity(request.Administrador);
                _dbContext.Usuarios.Add(entity);
                var id = entity.Id;
                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();
                _logger.LogInformation("AgregarAdministradorCommandHandler.HandleAsync {Response}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error AgregarAdministradorCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw;
            }
        }
    }
}
