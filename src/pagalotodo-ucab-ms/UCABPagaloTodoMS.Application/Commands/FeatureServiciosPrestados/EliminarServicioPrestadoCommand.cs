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
    /// Comando para eliminar un servicio prestado.
    /// </summary>
    public class EliminarServicioPrestadoCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece el identificador del servicio prestado a eliminar.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando EliminarServicioPrestadoCommand.
        /// </summary>
        /// <param name="id">El identificador del servicio prestado a eliminar.</param>
        public EliminarServicioPrestadoCommand(Guid id)
        {
            Id = id;
        }
    }
}
