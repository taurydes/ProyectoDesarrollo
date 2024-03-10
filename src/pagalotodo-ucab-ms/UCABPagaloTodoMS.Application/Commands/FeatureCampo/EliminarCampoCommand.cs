using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands.FeatureCampo
{

    /// <summary>
    /// Comando para eliminar un campo.
    /// </summary>
    public class EliminarCampoCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece el identificador del campo a eliminar.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando EliminarCampoCommand.
        /// </summary>
        /// <param name="id">El identificador del campo a eliminar.</param>
        public EliminarCampoCommand(Guid id)
        {
            Id = id;
        }
    }
}
