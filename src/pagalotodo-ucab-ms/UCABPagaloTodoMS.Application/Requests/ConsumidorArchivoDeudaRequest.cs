using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Requests
{
    /// <summary>
    /// Representa la solicitud de creación de una solicitud para saber si un consumidor tiene una deuda.
    /// </summary>
    public class ConsumidorArchivoDeudaRequest
    {
        /// <summary>
        /// Representa el Id del servicio del cual se está haciendo la consulta su archivo deuda
        /// </summary>
        public Guid ServicioPrestadoId { get; set; }

        /// <summary>
        /// Representa el Id del consumidor del cual se está haciendo la consulta su archivo deuda
        /// </summary>
        public Guid ConsumidorId { get; set; }
    }
}
