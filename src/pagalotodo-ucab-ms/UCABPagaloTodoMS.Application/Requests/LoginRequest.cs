using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Requests
{
    /// <summary>
    /// Representa la solicitud de inicio de sesión.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoginRequest
    {
        /// <summary>
        /// Obtiene o establece el nombre de usuario.
        /// </summary>
        public string NombreUsuario { get; set; }

        /// <summary>
        /// Obtiene o establece la contraseña.
        /// </summary>
        public string Clave { get; set; }
    }
}