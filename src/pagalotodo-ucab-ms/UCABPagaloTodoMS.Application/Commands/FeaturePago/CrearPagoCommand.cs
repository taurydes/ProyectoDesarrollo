using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;


namespace UCABPagaloTodoMS.Application.Commands.FeaturePago
{
    /// <summary>
    /// Comando para crear un nuevo pago.
    /// </summary>
    public class CrearPagoCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece la información del pago a crear.
        /// </summary>
        public CrearPagoRequest Pago { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando CrearPagoCommand.
        /// </summary>
        /// <param name="request">La información del pago a crear.</param>
        public CrearPagoCommand(CrearPagoRequest request)
        {
            Pago = request;
        }
    }
}
