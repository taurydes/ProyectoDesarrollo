using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Responses
{
    /// <summary>
    /// Clase que representa la respuesta de un administrador.
    /// Hereda de UsuarioResponse.
    /// </summary>
    public class AdministradorResponse : UsuarioResponse
    {
        /// <summary>
        /// Obtiene o establece el nombre del administrador.
        /// </summary>
        public string? NombreAdministrador { get; set; }
    }
}