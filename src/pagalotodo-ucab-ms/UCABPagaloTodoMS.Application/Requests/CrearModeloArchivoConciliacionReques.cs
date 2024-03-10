using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Requests
{
    /// <summary>
    /// Representa la solicitud de creación de un modelo de archivo de conciliación.
    /// </summary>
    public class CrearModeloArchivoConciliacionRequest
    {
        /// <summary>
        /// Obtiene o establece el nombre del modelo de archivo de conciliación.
        /// </summary>
        public string? NombreModelo { get; set; }

        /// <summary>
        /// Obtiene o establece un valor booleano que indica si el nombre del campo está presente en el modelo.
        /// </summary>
        public bool NombreCampo { get; set; }

        /// <summary>
        /// Obtiene o establece un valor booleano que indica si el tipo de campo está presente en el modelo.
        /// </summary>
        public bool Tipo { get; set; }

        /// <summary>
        /// Obtiene o establece un valor booleano que indica si la longitud del campo está presente en el modelo.
        /// </summary>
        public bool Longitud { get; set; }

        /// <summary>
        /// Obtiene o establece un valor booleano que indica si la referencia del campo está presente en el modelo.
        /// </summary>
        public bool Referencia { get; set; }

        /// <summary>
        /// Obtiene o establece un valor booleano que indica si el monto del campo está presente en el modelo.
        /// </summary>
        public bool Monto { get; set; }
    }
}