using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    /// <summary>
    /// Clase que representa la solicitud de creaci�n de una solicitud de pagos a un servicio prestado.
    /// </summary>
    public class PagosServicioPrestadoPorRangoModel
    {
        /// <summary>
        /// Obtiene o establece el identificador del servicio prestado.
        /// </summary>
        public Guid ServicioPrestadoId { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de inicio por la cual se va a filtrar la consulta.
        /// </summary>

        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de fin por la cual se va a filtrar la consulta.
        /// </summary>
        public DateTime FechaFin { get; set; }

        /// <summary>
        /// Obtiene o establece las condiciones que requerir� el archivo de conciliaci�n en cuanto a los campos que mostrar�
        /// </summary>
        public List<string>? CamposRequeridos { get; set; }
    }
}
