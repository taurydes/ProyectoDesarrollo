using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureConsumidor;
using UCABPagaloTodoMS.Application.Commands.FeaturePrestadorServicio;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeaturePrestadorServicio
{
    /// <summary>
    /// Clase que maneja el comando para actualizar un prestador de servicio.
    /// </summary>
    public class ActualizarPrestadorServicioCommandHandler : IRequestHandler<ActualizarPrestadorServicioCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ActualizarPrestadorServicioCommandHandler> _logger;

        public ActualizarPrestadorServicioCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ActualizarPrestadorServicioCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para actualizar un prestador de servicio.
        /// </summary>
        /// <param name="request">Comando para actualizar un prestador de servicio.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del prestador de servicio actualizado.</returns>
        public async Task<Guid> Handle(ActualizarPrestadorServicioCommand request, CancellationToken cancellationToken)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("ActualizarPrestadorServicioCommand.HandleAsync {Request}", request);
                var entity = await _dbContext.PrestadorServicios.FindAsync(request.Id);
                if (entity == null)
                {
                    _logger.LogWarning("ActualizarPrestadorServicioCommand.HandleAsync: Usuario no encontrado.");
                    return default;
                }
                //actualizar datos globales Usuario
                entity.NombreUsuario = request.Prestador.NombreUsuario;
                entity.Clave = request.Prestador.Clave;
                entity.Correo = request.Prestador.Correo;

                //Actualizar Datos propios del Prestador Servicio
                entity.NombreEmpresa = request.Prestador.NombreEmpresa;
                entity.Rif = request.Prestador.Rif;
               

                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();

                _logger.LogInformation("ActualizarPrestadorServicioCommand.HandleAsync {Response}", entity.Id);
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ActualizarPrestadorServicioCommand.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw;
            }
        }

    }
}
