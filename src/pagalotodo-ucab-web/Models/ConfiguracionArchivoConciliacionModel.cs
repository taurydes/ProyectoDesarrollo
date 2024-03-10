using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    /// <summary>
    /// Clase que representa la respuesta de un servicio prestado filtrando los pagos recibidos por rango.
    /// </summary>
    public class ConfiguracionArchivoConciliacionModel
    {
        /// <summary>
        /// Obtiene o establece el ID del servicio prestado.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del servicio prestado.
        /// </summary>
        public string? Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del servicio prestado.
        /// </summary>
        public string? NombreEmpresa { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de pagos realizados por el servicio.
        /// </summary>
        public List<PagoModel>? PagosPorServicioRealizados { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de inicio por la cual se va a filtrar la consulta.
        /// </summary>

        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de fin por la cual se va a filtrar la consulta.
        /// </summary>
        public DateTime FechaFin { get; set; }
    }
}
