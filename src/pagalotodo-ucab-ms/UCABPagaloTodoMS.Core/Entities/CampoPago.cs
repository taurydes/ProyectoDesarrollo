using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Core.Entities
{
    [ExcludeFromCodeCoverage]

    public class CampoPago : BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el ID del Pago al que pertenece el servicio campo.
        /// </summary>
        public Guid PagoId { get; set; } //Referencia al PagoID

        // <summary>
        /// Obtiene o establece el nombre del  campo del servicio prestado.
        /// </summary>
        public string? NombreCampo { get; set; }

        // <summary>
        /// Obtiene o establece el tipo del  campo del servicio prestado.
        /// </summary>
        public string? Tipo { get; set; }

        // <summary>
        /// Obtiene o establece la longitud del  campo del servicio prestado.
        /// </summary>
        public int? Longitud { get; set; }

        // <summary>
        /// Obtiene o establece el valor del  campo del servicio prestado.
        /// </summary>
        public string? Valor { get; set; }





    }

}
