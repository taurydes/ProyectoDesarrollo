using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Responses
{
    /// <summary>
    /// Clase que representa la respuesta de un pago.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PagoResponse
    {
        /// <summary>
        /// Obtiene o establece el identificador del pago.
        /// </summary>
        public Guid Id { get; set; }
       
        /// <summary>
        /// Obtiene o establece el identificador del servicio prestado asociado al pago.
        /// </summary>
        public Guid ServicioPrestadoId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del Consumidor  del prestado asociado al pago.
        /// </summary>
        public Guid ConsumidorId { get; set; }

        /// <summary>
        /// Obtiene o establece la referencia del pago.
        /// </summary>
        public string? Referencia { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del pago. los posibles estados son pendiente,aprobado y rechazado
        /// </summary>
        public string? EstadoPago { get; set; }

        /// <summary>
        /// Hace referencia a la lista de campos asociada al pago.
        /// </summary>
        public List<CampoPago>? CamposPago { get; set; }

    }
}
