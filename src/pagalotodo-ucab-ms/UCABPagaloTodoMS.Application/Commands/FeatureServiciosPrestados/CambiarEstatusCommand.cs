using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Commands.FeatureServiciosPrestados
{
    /// <summary>
    /// Comando para cambiar el estado de un servicio prestado.
    /// </summary>
    public class CambiarEstatusCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece el ID del servicio prestado.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Obtiene o establece la respuesta del servicio prestado.
        /// </summary>
        public ServicioPrestadoResponse Servicio { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando CambiarEstatusCommand.
        /// </summary>
        /// <param name="_servicio">La respuesta del servicio prestado.</param>
        /// <param name="id">El ID del servicio prestado.</param>
        public CambiarEstatusCommand(ServicioPrestadoResponse _servicio, Guid id)
        {
            Servicio = _servicio;
            Id = id;
        }
    }
    
}
