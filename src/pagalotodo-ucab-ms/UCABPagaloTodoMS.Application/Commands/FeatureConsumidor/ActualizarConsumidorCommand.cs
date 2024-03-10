using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands.FeatureConsumidor
{
    /// <summary>
    /// Comando para actualizar un consumidor.
    /// </summary>
    public class ActualizarConsumidorCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece el ID del consumidor.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Obtiene o establece la información actualizada del consumidor.
        /// </summary>
        public ConsumidorUpdateRequest Consumidor { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando ActualizarConsumidorCommand.
        /// </summary>
        /// <param name="consumidor">La información actualizada del consumidor.</param>
        /// <param name="id">El ID del consumidor.</param>
        public ActualizarConsumidorCommand(ConsumidorUpdateRequest usuario, Guid id)
        {
            Consumidor = usuario;
            Id = id;
        }
    }
    
}
