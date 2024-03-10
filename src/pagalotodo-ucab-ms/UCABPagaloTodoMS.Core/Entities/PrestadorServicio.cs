using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Core.Entities
{
    public class PrestadorServicio : Usuario
    {
        /// <summary>
        /// Obtiene o establece el nombre de la empresa del prestador de servicio.
        /// </summary>
        public string NombreEmpresa { get; set; }

        /// <summary>
        /// Obtiene o establece el Registro de Información Fiscal (RIF) del prestador de servicio.
        /// </summary>
        public string Rif { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de servicios prestados por el prestador de servicio.
        /// </summary>
        public List<ServicioPrestado>? ServiciosPrestados { get; set; }

        /// <summary>
        /// Obtiene o establece el estado de la cuenta del prestador de servicio.
        /// </summary>
        public bool EstatusCuenta { get; set; } = true;
    }
}