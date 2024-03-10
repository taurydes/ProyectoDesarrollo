using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Requests
{
    public class CrearCampoRequest
    {
        /// <summary>
        /// Obtiene o establece el Id de la configuracion de pago que pertenece un campo.
        /// </summary>
        public Guid ConfiguracionPagoId { get; set; }

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
        public string? valor { get; set; }

        /// <summary>
        /// Obtiene o establece si el campo es requerido.
        /// </summary>
        public bool Requerido { get; set; }
      

    }
}
