using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Requests
{
    /// <summary>
    /// Representa la solicitud de actualización de un administrador.
    /// </summary>
    public class AdministradorUpdateRequest : UsuarioRequest
    {
        /// <summary>
        /// Obtiene o establece el identificador del administrador.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del administrador.
        /// </summary>
        public string? NombreAdministrador { get; set; }
    }
}