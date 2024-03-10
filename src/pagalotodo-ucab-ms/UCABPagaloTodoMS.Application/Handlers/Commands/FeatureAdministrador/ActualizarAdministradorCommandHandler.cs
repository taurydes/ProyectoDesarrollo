using MediatR;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Commands.FeaturePrestadorServicio;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureAdministrador
{
    /// <summary>
    /// Manejador de comando para actualizar un administrador.
    /// </summary>
    public class ActualizarAdministradorCommandHandler : IRequestHandler<ActualizarAdministradorCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ActualizarAdministradorCommandHandler> _logger;

        /// <summary>
        /// Maneja el comando para actualizar un administrador.
        /// </summary>
        /// <param name="request">Comando de actualización del administrador.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del administrador actualizado.</returns>
        public ActualizarAdministradorCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ActualizarAdministradorCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Guid> Handle(ActualizarAdministradorCommand request, CancellationToken cancellationToken)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("ActualizarPrestadorServicioCommand.HandleAsync {Request}", request);
                var entity = await _dbContext.Administradores.FindAsync(request.Id);
                if (entity == null)
                {
                    _logger.LogWarning("ActualizarPrestadorServicioCommand.HandleAsync: Usuario no encontrado.");
                    return default;
                }
                //actualizar datos globales Usuario
                entity.NombreUsuario = request.Administrador.NombreUsuario;
                entity.Clave = request.Administrador.Clave;
                entity.Correo = request.Administrador.Correo;

                //Actualizar Datos propios del Prestador Servicio
                entity.NombreAdministrador = request.Administrador.NombreAdministrador;
                             

                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();

                _logger.LogInformation("ActualizarPrestadorServicioCommand.HandleAsync {Response}", entity.Id);
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ActualizarPrestadorServicioCommand.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                 throw new Exception($"No se encontró el administrador con ID {request.Id} en la base de datos.", ex);
            }
        }

    }
}
