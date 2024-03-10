using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureServiciosPrestados;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeatureServiciosPrestados
{
    /// <summary>
    /// Clase que maneja el comando para crear un servicio prestado.
    /// </summary>
    public class CrearServicioPrestadoCommandHandler : IRequestHandler<CrearServicioPrestadoCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<CrearServicioPrestadoCommandHandler> _logger;

        /// <summary>
        /// Constructor de la clase CrearServicioPrestadoCommandHandler.
        /// </summary>
        /// <param name="dbContext">Contexto de la base de datos.</param>
        /// <param name="logger">Instancia del logger.</param>
        public CrearServicioPrestadoCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<CrearServicioPrestadoCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Maneja el comando para crear un servicio prestado.
        /// </summary>
        /// <param name="request">Comando para crear un servicio prestado.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del servicio prestado creado.</returns>
        public async Task<Guid> Handle(CrearServicioPrestadoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Servicio == null)
                {
                    _logger.LogWarning("CrearServicioPrestadoCommandHandler.Handle: Request nulo.");
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
        /// Maneja el comando de creación de un servicio prestado de forma asíncrona.
        /// </summary>
        /// <param name="request">Comando para crear un servicio prestado.</param>
        /// <returns>Identificador del servicio prestado creado.</returns>
        private async Task<Guid> HandleAsync(CrearServicioPrestadoCommand request)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("CrearServicioPrestadoCommandHandler.HandleAsync {Request}", request);
                var entity = ServicioPrestadoMapper.MapRequestServicioPrestadoEntity(request.Servicio);
                _dbContext.ServiciosPrestados.Add(entity);
                var id = entity.Id;
                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();
                _logger.LogInformation("CrearServicioPrestadoCommandHandler.HandleAsync {Response}", id);
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error CrearServicioPrestadoCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw new Exception($"No se encontró el Servicio con ID {request.Servicio.Id} en la base de datos.", ex);

            }
        }
    }
}
