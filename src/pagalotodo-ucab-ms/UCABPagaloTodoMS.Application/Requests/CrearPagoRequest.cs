using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Requests
{
    /// <summary>
    /// Representa la solicitud de creación de un pago.
    /// </summary>
    public class CrearPagoRequest
    {
        /// <summary>
        /// Obtiene o establece el identificador del servicio prestado asociado al pago.
        /// </summary>
        public Guid ServicioPrestadoId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del consumidor asociado al pago.
        /// </summary>
        public Guid ConsumidorId { get; set; }

        /// <summary>
        /// Obtiene o establece la referencia del pago.
        /// </summary>
        public string? Referencia { get; set; }

    }
}
