using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Core.Entities
{
    /// <summary>
    /// Representa un archivo de conciliación en el sistema.
    /// Hereda de la clase base BaseEntity.
    /// </summary>
    public class ArchivoConciliacion : BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador del servicio.
        /// </summary>
        public Guid ServicioPrestadoId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el archivo de conciliación ha sido confirmado como correcto.
        /// </summary>
        public bool Confirmado { get; set; } = false; // se confirma si está correcto o no 


        /// <summary>
        /// Obtiene o establece la descripción del servicio prestado.
        /// </summary>
        public string? UrlDescarga { get; set; }

    }
}
