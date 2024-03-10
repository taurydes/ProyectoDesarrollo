using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Requests
{
    /// <summary>
    /// Clase que representa la solicitud de actualización de un servicio prestado.
    /// </summary>
    public class ServicioPrestadoUpdateRequest
    {
        /// <summary>
        /// Obtiene o establece el identificador del servicio prestado.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del servicio prestado.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del servicio prestado.
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Obtiene o establece el costo del servicio prestado.
        /// </summary>
        public float Costo { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del servicio prestado (activo, inactivo, pronto).
        /// </summary>
        public string EstadoServicio { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de pago del servicio prestado.
        /// </summary>
        public bool TipoPago { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del archivo de conciliación asociado al servicio.
        /// </summary>
        public Guid ArchivoConciliacionId { get; set; }
    }
}