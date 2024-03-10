using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureConsumidor;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands
{
    /// <summary>
    /// Clase que maneja el comando para actualizar un consumidor.
    /// </summary>
    public class ActualizarConsumidorCommandHandler : IRequestHandler<ActualizarConsumidorCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ActualizarConsumidorCommandHandler> _logger;

       
        public ActualizarConsumidorCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ActualizarConsumidorCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para actualizar un consumidor.
        /// </summary>
        /// <param name="request">Comando para actualizar un consumidor.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del consumidor actualizado.</returns>
        public async Task<Guid> Handle(ActualizarConsumidorCommand request, CancellationToken cancellationToken)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("ActualizarUsuarioCommandHandler.HandleAsync {Request}", request);
                var entity = await _dbContext.Consumidores.FindAsync(request.Id);
                if (entity == null)
                {
                    _logger.LogWarning("ActualizarUsuarioCommandHandler.HandleAsync: Usuario no encontrado.");
                    return default;
                }
                //actualizar datos globales Usuario
                entity.NombreUsuario = request.Consumidor.NombreUsuario;
                entity.Clave = request.Consumidor.Clave;
                entity.Correo = request.Consumidor.Correo;
                //Actualizar Datos propios del consumidor
                entity.Nombre = request.Consumidor.Nombre;
                entity.Apellido = request.Consumidor.Apellido;
                entity.Telefono = request.Consumidor.Telefono;
                entity.Direccion = request.Consumidor.Direccion;

                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();

                _logger.LogInformation("ActualizarUsuarioCommandHandler.HandleAsync {Response}", entity.Id);
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ActualizarUsuarioCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw;
            }
        }

    }
}
