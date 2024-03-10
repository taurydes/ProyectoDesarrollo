using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Core.Entities
{
    /// <summary>
    /// Representa un archivo de deuda en el sistema.
    /// Hereda de la clase base BaseEntity.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ArchivoDeuda : BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador del servicio.
        /// </summary>
        public Guid ServicioPrestadoId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del consumidor asociado al archivo de deuda.
        /// </summary>
        public Guid ConsumidorId { get; set; }

    }
}