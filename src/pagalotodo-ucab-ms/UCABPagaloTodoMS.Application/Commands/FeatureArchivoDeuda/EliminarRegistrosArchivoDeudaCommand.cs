using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands
{
    /// <summary>
    /// Comando para eliminar todos los registros de un servicio de archivo deuda para volver a agregar nuevos.
    /// </summary>
    public class EliminarRegistrosArchivoDeudaCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece el identificador del servicio a eliminar.
        /// </summary>
        public Guid ServicioId { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando EliminarRegistrosArchivoDeudaCommand.
        /// </summary>
        /// <param name="_servicioId">El identificador del servicio a eliminar.</param>
        public EliminarRegistrosArchivoDeudaCommand(Guid _servicioId)
        {
            ServicioId = _servicioId;
        }
    }
}
