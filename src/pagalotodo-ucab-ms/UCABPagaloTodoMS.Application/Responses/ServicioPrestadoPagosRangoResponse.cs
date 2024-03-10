using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Responses
{
    /// <summary>
    /// Clase que representa la respuesta de un servicio prestado filtrando los pagos recibidos por rango.
    /// </summary>
    public class ServicioPrestadoPagosRangoResponse 
    {
        /// <summary>
        /// Obtiene o establece el ID del servicio prestado.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del servicio prestado.
        /// </summary>
        public string? Nombre { get; set; } 
        
        /// <summary>
        /// Obtiene o establece el nombre del servicio prestado.
        /// </summary>
        public string? NombreEmpresa { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de pagos realizados por el servicio.
        /// </summary>
        public List<Pago>? PagosPorServicioRealizados { get; set; }

        /// <summary>
        /// Obtiene o establece el correo electrónico del usuario.
        /// </summary>
        public string? Correo { get; set; }


    }
}
