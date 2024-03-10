using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Core.Entities
{
    public class Pago : BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador del consumidor al que pertenece el pago.
        /// </summary>
        public Guid ConsumidorId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del servicio prestado al que pertenece el pago.
        /// </summary>
        public Guid ServicioPrestadoId { get; set; }

        /// <summary>
        /// Obtiene o establece la referencia del pago.
        /// </summary>
        public string? Referencia { get; set; }

      
        /// <summary>
        /// Obtiene o establece la lista de campos que conforman el pago.
        /// </summary>
        public List<CampoPago>? CamposPago { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del pago. los posibles estados son pendiente,aprobado y rechazado
        /// </summary>
        public string? EstadoPago { get; set; } = "pendiente";

    }
}
