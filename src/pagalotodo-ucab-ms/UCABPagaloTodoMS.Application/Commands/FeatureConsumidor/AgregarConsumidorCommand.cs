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
    /// Comando para agregar un nuevo consumidor.
    /// </summary>
    public class AgregarConsumidorCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece la información del consumidor a agregar.
        /// </summary>
        public ConsumidorRequest _request { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando AgregarConsumidorCommand.
        /// </summary>
        /// <param name="request">La información del consumidor a agregar.</param>
        public AgregarConsumidorCommand(ConsumidorRequest request)
        {
            _request = request;
        }
       
    }
}
