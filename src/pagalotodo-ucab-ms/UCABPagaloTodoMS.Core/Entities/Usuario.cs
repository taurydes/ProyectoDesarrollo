using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Core.Entities
{
    public class Usuario : BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el nombre de usuario.
        /// </summary>
        public string? NombreUsuario { get; set; }

        /// <summary>
        /// Obtiene o establece la clave del usuario.
        /// </summary>
        public string? Clave { get; set; }

        /// <summary>
        /// Obtiene o establece el correo electrónico del usuario.
        /// </summary>
        public string? Correo { get; set; }
    }
}