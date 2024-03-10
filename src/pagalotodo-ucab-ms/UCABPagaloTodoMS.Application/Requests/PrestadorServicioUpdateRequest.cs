using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Requests
{
    public class PrestadorServicioUpdateRequest : UsuarioRequest
    {
        /// <summary>
        /// Obtiene o establece el identificador del proveedor de servicios.
        /// </summary>
        public Guid Id { get; set; }

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