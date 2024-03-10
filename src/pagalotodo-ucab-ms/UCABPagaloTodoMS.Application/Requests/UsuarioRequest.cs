using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Requests
{
    /// <summary>
    /// Clase base que representa la solicitud de un usuario.
    /// </summary>
    public class UsuarioRequest
    {
        /// <summary>
        /// Obtiene o establece el identificador del usuario.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de usuario.
        /// </summary>
        public string? NombreUsuario { get; set; }

        /// <summary>
        /// Obtiene o establece la contraseña del usuario.
        /// </summary>
        public string? Clave { get; set; }

        /// <summary>
        /// Obtiene o establece el correo electrónico del usuario.
        /// </summary>
        public string? Correo { get; set; }
    }
}