using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Requests
{
    /// <summary>
    /// Representa la solicitud de creación de un servicio prestado.
    /// </summary>
    public class CrearServicioPrestadoRequest
    {
        /// <summary>
        /// Obtiene o establece el identificador del servicio prestado.
        /// </summary>
        public Guid Id { get; set; } // Agregado. Comentario: ¿Es necesario tener esta propiedad?

        /// <summary>
        /// Obtiene o establece el identificador del prestador de servicios asociado al servicio prestado.
        /// </summary>
        public Guid PrestadorServicioId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del servicio prestado.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del servicio prestado.
        /// </summary>
        public string? Descripcion { get; set; }

        /// <summary>
        /// Obtiene o establece el costo del servicio prestado.
        /// </summary>
        public float Costo { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del servicio prestado. Puede ser "activo", "inactivo" o "pronto".
        /// </summary>
        public string? EstadoServicio { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de pago del servicio prestado. Se acordó utilizar un valor booleano.
        /// </summary>
        public bool TipoPago { get; set; }
    }
}