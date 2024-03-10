using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands.FeatureServicioPrestado
{
    /// <summary>
    /// Comando para actualizar un servicio prestado.
    /// </summary>
    public class ActualizarServicioPrestadoCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece el ID del servicio prestado.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Obtiene o establece la solicitud de actualización del servicio prestado.
        /// </summary>
        public ServicioPrestadoUpdateRequest Servicio { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando ActualizarServicioPrestadoCommand.
        /// </summary>
        /// <param name="_request">La solicitud de actualización del servicio prestado.</param>
        /// <param name="id">El ID del servicio prestado.</param>
        public ActualizarServicioPrestadoCommand(ServicioPrestadoUpdateRequest _request, Guid id)
        {
            Servicio = _request;
            Id = id;
        }
    }
}
