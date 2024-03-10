using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Core.Entities
{
    [ExcludeFromCodeCoverage]
    /// <summary>
    /// Representa un campo en un archivo de conciliación.
    /// </summary>

    public class Campo : BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el nombre del campo.
        /// </summary>
        public string? NombreCampo { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de dato del campo.
        /// </summary>
        public string? Tipo { get; set; }

        /// <summary>
        /// Obtiene o establece la longitud del campo.
        /// </summary>
        public int? Longitud { get; set; }

        /// <summary>
        /// Obtiene o establece si el campo es requerido.
        /// </summary>
        public bool Requerido { get; set; }

        /// <summary>
        /// Obtiene o establece la Configuracion de pago del campo.
        /// </summary>
        public Guid ConfiguracionPagoId { get; set; } //Referencia a la configuración 




    }

}
