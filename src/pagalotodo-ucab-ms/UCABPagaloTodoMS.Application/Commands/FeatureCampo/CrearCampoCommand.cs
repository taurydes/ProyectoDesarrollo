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
    /// Comando para agregar un nuevo campo.
    /// </summary>
    public class CrearCampoCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece la información del campo a agregar.
        /// </summary>
        public CrearCampoRequest Campo { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando CrearCampoCommand.
        /// </summary>
        /// <param name="request">La información del campo a agregar.</param>
        public CrearCampoCommand(CrearCampoRequest request)
        {
            Campo = request;
        }
    }
}
