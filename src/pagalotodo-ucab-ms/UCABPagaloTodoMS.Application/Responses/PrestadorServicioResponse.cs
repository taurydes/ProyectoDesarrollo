using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Responses
{
    /// <summary>
    /// Clase que representa la respuesta de un prestador de servicio.
    /// </summary>
    public class PrestadorServicioResponse : UsuarioResponse
    {
        /// <summary>
        /// Obtiene o establece el nombre de la empresa del prestador de servicio.
        /// </summary>
        public string? NombreEmpresa { get; set; }

        /// <summary>
        /// Obtiene o establece el RIF del prestador de servicio.
        /// </summary>
        public string? Rif { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de servicios prestados por el prestador de servicio.
        /// </summary>
        public List<ServicioPrestado>? ServiciosPrestados { get; set; }

        /// <summary>
        /// Obtiene o establece el estatus de la cuenta del prestador de servicio.
        /// </summary>
        public bool? EstatusCuenta { get; set; }
    }
}