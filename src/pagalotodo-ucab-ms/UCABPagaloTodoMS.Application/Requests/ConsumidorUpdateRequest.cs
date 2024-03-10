using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Requests
{
    /// <summary>
    /// Representa la solicitud de actualización de un consumidor.
    /// </summary>
    public class ConsumidorUpdateRequest : UsuarioRequest
    {
        /// <summary>
        /// Obtiene o establece el identificador del consumidor.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del consumidor.
        /// </summary>
        public string? Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el apellido del consumidor.
        /// </summary>
        public string? Apellido { get; set; }

        /// <summary>
        /// Obtiene o establece el número de teléfono del consumidor.
        /// </summary>
        public int? Telefono { get; set; }

        /// <summary>
        /// Obtiene o establece la dirección del consumidor.
        /// </summary>
        public string? Direccion { get; set; }

        /// <summary>
        /// Obtiene o establece el estatus de la cuenta del consumidor.
        /// </summary>
        public bool? EstatusCuenta { get; set; }
    }
}