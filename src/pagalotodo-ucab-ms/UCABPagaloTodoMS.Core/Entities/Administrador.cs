using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Core.Entities
{
    /// <summary>
    /// Representa un administrador en el sistema.
    /// Hereda de la clase base Usuario.
    /// </summary>
    public class Administrador : Usuario
    {
        /// <summary>
        /// Obtiene o establece el nombre del administrador.
        /// </summary>
        public string? NombreAdministrador { get; set; }
    }
}