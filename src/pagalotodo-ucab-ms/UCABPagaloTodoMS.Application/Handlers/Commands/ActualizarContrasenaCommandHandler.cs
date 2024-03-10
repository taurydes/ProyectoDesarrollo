using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Commands.FeatureConsumidor;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands
{
    /// <summary>
    /// Clase que maneja el comando para actualizar un la clave de un usuario.
    /// </summary>
    public class ActualizarContrasenaCommandHandler : IRequestHandler<ActualizarContrasenaCommand, bool>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ActualizarContrasenaCommandHandler> _logger;

       
        public ActualizarContrasenaCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ActualizarContrasenaCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para actualizar la clave de un usuario.
        /// </summary>
        /// <param name="request">Comando para actualizar la clave de un usuario.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>respuesta boolena sobre la actualización de la clave.</returns>
        public async Task<bool> Handle(ActualizarContrasenaCommand request, CancellationToken cancellationToken)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("ActualizarContrasenaCommandHandler.HandleAsync {Request}", request);
                var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Correo == request.DatosUsuario.Correo);
                if (usuario == null)
                {
                    _logger.LogWarning("ActualizarContrasenaCommandHandler.HandleAsync: Usuario no encontrado.");
                    return default;
                }
                //actualizar datos globales Usuario
                usuario.Clave = request.DatosUsuario.Clave;

                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();

                _logger.LogInformation("ActualizarContrasenaCommandHandler.HandleAsync {Response}", usuario.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ActualizarContrasenaCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw new UsuarioNotFoundException("Usuario no encontrado.");
            }
        }

    }
}
