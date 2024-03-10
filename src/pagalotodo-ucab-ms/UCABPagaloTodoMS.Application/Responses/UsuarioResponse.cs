using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Responses
{
    /// <summary>
    /// Clase base que representa la respuesta de un usuario.
    /// </summary>
    public class UsuarioResponse
    {
        /// <summary>
        /// Obtiene o establece el ID del usuario.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de usuario.
        /// </summary>
        public string NombreUsuario { get; set; }

        /// <summary>
        /// Obtiene o establece la clave del usuario.
        /// </summary>
        public string Clave { get; set; }

        /// <summary>
        /// Obtiene o establece el correo electrónico del usuario.
        /// </summary>
        public string Correo { get; set; }
    }
}