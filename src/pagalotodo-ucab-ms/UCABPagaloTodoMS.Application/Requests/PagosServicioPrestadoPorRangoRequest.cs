using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Requests
{
    /// <summary>
    /// Clase que representa la solicitud de creación de una solicitud de pagos a un servicio prestado.
    /// </summary>
    public class PagosServicioPrestadoPorRangoRequest
    {
        /// <summary>
        /// Obtiene o establece el identificador del servicio prestado.
        /// </summary>
        public Guid ServicioPrestadoId { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de inicio por la cual se va a filtrar la consulta.
        /// </summary>
        
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de fin por la cual se va a filtrar la consulta.
        /// </summary>
        public DateTime FechaFin { get; set; }
    }
}