using MassTransit.Initializers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeaturePago;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Services;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Application.Handlers.Commands.FeaturePago
{
    /// <summary>
    /// Clase que maneja el comando para crear un pago.
    /// </summary>
    public class CrearPagoCommandHandler : IRequestHandler<CrearPagoCommand, Guid>
    {
        private readonly IUCABPagaloTodoDbContext _dbContext;
        private readonly ILogger<CrearPagoCommandHandler> _logger;
        private readonly IMailService _mailService;

        public CrearPagoCommandHandler(IUCABPagaloTodoDbContext dbContext, ILogger<CrearPagoCommandHandler> logger, IMailService mailService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mailService = mailService;
        }

        /// <summary>
        /// Maneja el comando para crear un pago.
        /// </summary>
        /// <param name="request">Comando para crear un pago.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Identificador del pago creado.</returns>
        public async Task<Guid> Handle(CrearPagoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Pago == null)
                {
                    _logger.LogWarning("CrearServicioPrestadoCommandHandler.Handle: Request nulo.");
                    throw new PagoNullException("El objeto Pago no puede ser nulo.");
                }
                else
                {
                    return await HandleAsync(request);
                }
            }
            catch (Exception)
            {
                throw new PagoNullException("El objeto Pago no puede ser nulo.");
            }
        }

        /// <summary>
        /// Maneja el comando para crear un pago de forma asíncrona.
        /// </summary>
        /// <param name="request">Comando para crear un pago.</param>
        /// <returns>Identificador del pago creado.</returns>
        private async Task<Guid> HandleAsync(CrearPagoCommand request)
        {
            var transaction = _dbContext.BeginTransaction();
            try
            {
                _logger.LogInformation("CrearServicioPrestadoCommandHandler.HandleAsync {Request}", request);
                var entity = PagoMapper.MapRequestPagoEntity(request.Pago);
                _dbContext.Pagos.Add(entity);
                var id = entity.Id;
                await _dbContext.SaveEfContextChanges("APP");
                transaction.Commit();
                _logger.LogInformation("CrearServicioPrestadoCommandHandler.HandleAsync {Response}", id);

                try
                {
                    var correoUsuario = await _dbContext.Usuarios
                    .Where(u => u.Id == request.Pago.ConsumidorId)
                    .Select(u => u.Correo)
                    .FirstOrDefaultAsync();
                    var asuntoMensaje = "PagalotodoUcab. Registro de Pago";
                    var cuerpoMensaje = $"Su pago se ha procesado de manera exitosa. Número de referencia: {request.Pago.Referencia}.";

                    // Envía el correo electrónico con el código y el enlace a la página de cambiarContraseña
                    await _mailService.EnviarCorreoElectronicoAsync(correoUsuario ?? "dtoro1996@gmail.com", asuntoMensaje, cuerpoMensaje);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error CrearServicioPrestadoCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                    transaction.Rollback();
                    throw;
                }


                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error CrearServicioPrestadoCommandHandler.HandleAsync. {Mensaje}", ex.Message);
                transaction.Rollback();
                throw;
            }
        }
    }
}
