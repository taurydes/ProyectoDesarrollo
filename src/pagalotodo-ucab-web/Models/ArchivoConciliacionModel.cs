using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class ArchivoConciliacionModel
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
        /// Obtiene o establece un valor que indica si el archivo de conciliaci�n ha sido confirmado como correcto.
        /// </summary>
        public bool Confirmado { get; set; } // se confirma si est� correcto o no 


        /// <summary>
        /// Obtiene o establece la descripci�n del servicio prestado.
        /// </summary>
        public string? UrlDescarga { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora de creaci�n de la entidad.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
