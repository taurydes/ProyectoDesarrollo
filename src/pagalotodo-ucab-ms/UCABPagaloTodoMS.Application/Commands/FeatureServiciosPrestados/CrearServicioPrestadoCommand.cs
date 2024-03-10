using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands.FeatureServiciosPrestados
{
    /// <summary>
    /// Comando para crear un servicio prestado.
    /// </summary>
    public class CrearServicioPrestadoCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece la solicitud para crear el servicio prestado.
        /// </summary>
        public CrearServicioPrestadoRequest Servicio { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando CrearServicioPrestadoCommand.
        /// </summary>
        /// <param name="request">La solicitud para crear el servicio prestado.</param>
        public CrearServicioPrestadoCommand(CrearServicioPrestadoRequest request)
        {
            Servicio = request;
        }
    }
}
