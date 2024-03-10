using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Responses
{
    /// <summary>
    /// Clase que representa la respuesta de una configuración de pago.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ConfiguracionPagoResponse
    {
        /// <summary>
        /// Obtiene o establece el ID de la configuración de pago.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Obtiene o establece el ID del servicio prestado al que pertenece la configuración de pago.
        /// </summary>
        public Guid? ServicioPrestadoId { get; set; }

        /// <summary>
        /// lista de campos que pertenecen a la configuracion de pago del servicio prestado.
        /// </summary>
        public List<Campo>? Campos { get; set; }

    }
}
