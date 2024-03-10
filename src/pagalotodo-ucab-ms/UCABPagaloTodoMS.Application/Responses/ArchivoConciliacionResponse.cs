using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Responses
{
    /// <summary>
    /// Clase que representa la respuesta de un archivo Conciliacion.
    /// </summary>
    public class ArchivoConciliacionResponse 
    {
        /// <summary>
        /// Obtiene o establece el identificador del archivo.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del servicio.
        /// </summary>
        public Guid ServicioPrestadoId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el archivo de conciliación ha sido confirmado como correcto.
        /// </summary>
        public bool Confirmado { get; set; } // se confirma si está correcto o no 


        /// <summary>
        /// Obtiene o establece la descripción del servicio prestado.
        /// </summary>
        public string? UrlDescarga { get; set; }

    }
}