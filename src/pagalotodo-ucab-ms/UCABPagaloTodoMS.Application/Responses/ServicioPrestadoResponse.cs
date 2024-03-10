using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Responses
{
    /// <summary>
    /// Clase que representa la respuesta de un servicio prestado.
    /// </summary>
    public class ServicioPrestadoResponse
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
        /// Obtiene o establece la descripción del servicio prestado.
        /// </summary>
        public string? Descripcion { get; set; }

        /// <summary>
        /// Obtiene o establece el costo del servicio prestado.
        /// </summary>
        public float Costo { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del servicio prestado.
        /// Puede ser "activo", "inactivo" o "pronto".
        /// </summary>
        public string? EstadoServicio { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de pago del servicio prestado.
        /// </summary>
        public bool TipoPago { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de pagos realizados por el servicio.
        /// </summary>
        public List<Pago>? PagosPorServicioRealizados { get; set; }

        /// <summary>
        /// Obtiene o establece el ID del Configuracion de pago del servicio prestado.
        /// </summary>
        public Guid ConfiguracionPagoId { get; set; }

        /// <summary>
        /// Obtiene o establece el ID del Prestador de del servicio prestado.
        /// </summary>
        public Guid PrestadorServicioId { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de archivos de conciliación utilizados por el servicio.
        /// </summary>
        public List<ArchivoConciliacion>? ArchivosPertenecientes { get; set; }

        /// <summary>
        /// Obtiene o establece el estatus del servicio.
        /// </summary>
        public bool EstatusServicio { get; set; }


    }
}
