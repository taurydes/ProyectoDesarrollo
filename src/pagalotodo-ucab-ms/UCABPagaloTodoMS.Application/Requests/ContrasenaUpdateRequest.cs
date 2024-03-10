using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Requests
{
    public class ContrasenaUpdateRequest 
    {

        /// <summary>
        /// Obtiene o establece el correo electrónico del usuario.
        /// </summary>
        public string? Correo { get; set; }

        /// <summary>
        /// Obtiene o establece la contraseña del usuario. por defecto la clave Admin123.
        /// </summary>
        public string? Clave { get; set; } = "Admin123";
    }
}