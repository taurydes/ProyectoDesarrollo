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
    /// Comando para eliminar un usuario.
    /// </summary>
    public class EliminarUsuarioCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece el identificador del usuario a eliminar.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando EliminarUsuarioCommand.
        /// </summary>
        /// <param name="id">El identificador del usuario a eliminar.</param>
        public EliminarUsuarioCommand(Guid id)
        {
            Id = id;
        }
    }
}
