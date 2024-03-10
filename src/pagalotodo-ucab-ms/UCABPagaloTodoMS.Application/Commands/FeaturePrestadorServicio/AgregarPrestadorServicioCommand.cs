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
    /// Comando para agregar un nuevo prestador de servicio.
    /// </summary>
    public class AgregarPrestadorServicioCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece la información del prestador de servicio a agregar.
        /// </summary>
        public PrestadorServicioRequest Prestador { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando AgregarPrestadorServicioCommand.
        /// </summary>
        /// <param name="request">La información del prestador de servicio a agregar.</param>
        public AgregarPrestadorServicioCommand(PrestadorServicioRequest request)
        {
            Prestador = request;
        }
    }
}
