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
using UCABPagaloTodoMS.Application.Commands.FeatureServicioPrestado;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureServiciosPrestados
{
    /// <summary>
    /// Clase que maneja el comando para actualizar un servicio prestado.
    /// </summary>
    public class ActualizarServicioPrestadoCommandHandler : IRequestHandler<ActualizarServicioPrestadoCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<ActualizarServicioPrestadoCommandHandler> _logger;

        public ActualizarServicioPrestadoCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ActualizarServicioPrestadoCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para actualizar un servicio prestado.
        /// </summary>
        /// <param name="request">Comando para actualizar un servicio prestado.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del servicio prestado actualizado.</returns>
        public async Task<Guid> Handle(ActualizarServicioPrestadoCommand request, CancellationToken cancellationToken)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("ActualizarServicioPrestadoCommandHandler.HandleAsync {Request}", request);
                var entity = await _dbContext.ServiciosPrestados.FindAsync(request.Id);
                if (entity == null)
                {
                    _logger.LogWarning("ActualizarServicioPrestadoCommandHandler.HandleAsync: Usuario no encontrado.");
                    return default;
                }
                //actualizar datos del Referentes solo al Servicio
                entity.Nombre = request.Servicio.Nombre;
                entity.Descripcion = request.Servicio.Descripcion;
                entity.Costo = request.Servicio.Costo;
                entity.EstadoServicio = request.Servicio.EstadoServicio;
                entity.TipoPago = request.Servicio.TipoPago;
                



        await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();

                _logger.LogInformation("ActualizarServicioPrestadoCommandHandler.HandleAsync {Response}", entity.Id);
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ActualizarServicioPrestadoCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw;
            }
        }

    }
}
