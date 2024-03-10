using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Requests
{
    /// <summary>
    /// Representa una solicitud para crear un proveedor de servicios.
    /// </summary>
    public class PrestadorServicioRequest : UsuarioRequest
    {
        /// <summary>
        /// Obtiene o establece el nombre de la empresa del proveedor de servicios.
        /// </summary>
        public string? NombreEmpresa { get; set; }

        /// <summary>
        /// Obtiene o establece el RIF (Registro de Información Fiscal) del proveedor de servicios.
        /// </summary>
        public string? Rif { get; set; }
    }
}