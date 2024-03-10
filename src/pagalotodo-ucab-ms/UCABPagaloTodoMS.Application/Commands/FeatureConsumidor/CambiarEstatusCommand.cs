using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Commands.FeatureConsumidor
{
    /// <summary>
    /// Obtiene o establece el ID del consumidor.
    /// </summary>
    public class CambiarEstatusCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece el ID del consumidor.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Obtiene o establece la información actualizada del consumidor.
        /// </summary>
        public ConsumidorResponse Consumidor { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando CambiarEstatusCommand.
        /// </summary>
        /// <param name="usuario">La información actualizada del consumidor.</param>
        /// <param name="id">El ID del consumidor.</param>
        public CambiarEstatusCommand(ConsumidorResponse usuario, Guid id)
        {
            Consumidor = usuario;
            Id = id;
        }
    }
    
}
