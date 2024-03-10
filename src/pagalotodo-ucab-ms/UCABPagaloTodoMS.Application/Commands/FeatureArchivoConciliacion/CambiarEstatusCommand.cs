using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Commands.FeatureArchivoConciliacion
{
    /// <summary>
    /// Obtiene o establece el ID del archivo conciliacion.
    /// </summary>
    public class CambiarEstatusCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece el ID del archivo conciliacion.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Obtiene o establece la información actualizada del consumidor.
        /// </summary>
        public ArchivoConciliacionResponse Archivo { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando CambiarEstatusCommand.
        /// </summary>
        /// <param name="_archivo">La información actualizada del archivo.</param>
        /// <param name="id">El ID del consumidor.</param>
        public CambiarEstatusCommand(ArchivoConciliacionResponse _archivo, Guid id)
        {
            Archivo = _archivo;
            Id = id;
        }
    }
    
}
