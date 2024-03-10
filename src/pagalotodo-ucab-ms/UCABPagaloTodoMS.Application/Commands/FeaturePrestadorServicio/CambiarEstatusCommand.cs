using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Commands.FeaturePrestadorServicio
{
    /// <summary>
    /// Comando para cambiar el estatus de un prestador de servicio.
    /// </summary>
    public class CambiarEstatusCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece el ID del prestador de servicio.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Obtiene o establece la respuesta del prestador de servicio.
        /// </summary>
        public PrestadorServicioResponse prestador { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando CambiarEstatusCommand.
        /// </summary>
        /// <param name="usuario">La respuesta del prestador de servicio.</param>
        /// <param name="id">El ID del prestador de servicio.</param>
        public CambiarEstatusCommand(PrestadorServicioResponse usuario, Guid id)
        {
            prestador = usuario;
            Id = id;
        }
    }
    
}
