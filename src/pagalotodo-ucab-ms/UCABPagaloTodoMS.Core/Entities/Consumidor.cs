using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Core.Entities
{
    public class Consumidor : Usuario
    {
        /// <summary>
        /// Obtiene o establece la cedula de un consumidor.
        /// </summary>
        public int Cedula { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del consumidor.
        /// </summary>
        public string? Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el apellido del consumidor.
        /// </summary>
        public string? Apellido { get; set; }

        /// <summary>
        /// Obtiene o establece el número de teléfono del consumidor.
        /// </summary>
        public int? Telefono { get; set; }

        /// <summary>
        /// Obtiene o establece la dirección del consumidor.
        /// </summary>
        public string? Direccion { get; set; }

        /// <summary>
        /// Obtiene o establece el estado de la cuenta del consumidor.
        /// </summary>
        public bool EstatusCuenta { get; set; } = true;

        /// <summary>
        /// Obtiene o establece la lista de pagos realizados por el consumidor.
        /// </summary>
        public List<Pago>? PagosRealizados { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de pagos pendientes del consumidor.
        /// </summary>
        public Guid? ArchivoDeudaId { get; set; }
    }
}