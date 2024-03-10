using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Requests
{
    /// <summary>
    /// Representa la solicitud de creación o actualización de un campo.
    /// </summary>
    public class CrearCampoPagoRequest
    {
        /// <summary>
        /// Obtiene o establece el id del pago al cual pertenece el campo.
        /// </summary>
        public Guid PagoId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del campo.
        /// </summary>
        public string? NombreCampo { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo del campo.
        /// </summary>
        public string? Tipo { get; set; }

        /// <summary>
        /// Obtiene o establece la longitud del campo.
        /// </summary>
        public int? Longitud { get; set; }

        /// <summary>
        /// Obtiene o establece el valor del campo.
        /// </summary>
        public string? Valor { get; set; }
      

    }
}
