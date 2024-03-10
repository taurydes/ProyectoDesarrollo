using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Core.Entities
{
    public class ServicioPrestado : BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el ID del prestador de servicio al que pertenece el servicio prestado.
        /// </summary>
        public Guid PrestadorServicioId { get; set; }

        /// <summary>
        /// Obtiene o establece el ID de la configuración de pago de servicio al que pertenece el servicio prestado.
        /// </summary>
        public Guid ConfiguracionPagoId { get; set; }

          /// <summary>
          /// Lista de archivos de conciliación que se han generado para este servicio
          /// </summary>
        public List<ArchivoConciliacion>? ArchivosPertenecientes { get; set; }

        // <summary>
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
        /// Obtiene o establece el estado del servicio.
        /// Los posibles valores son: "activo", "inactivo" o "pronto".
        /// </summary>
        public string? EstadoServicio { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de pago del servicio.
        /// Si es true, se requiere pago; si es false, no se requiere pago.
        /// </summary>
        public bool TipoPago { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de pagos realizados por el servicio.
        /// </summary>
        public List<Pago>? PagosPorServicioRealizados { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del servicio.
        /// </summary>
        public bool EstatusServicio { get; set; } = true;








    }
}
