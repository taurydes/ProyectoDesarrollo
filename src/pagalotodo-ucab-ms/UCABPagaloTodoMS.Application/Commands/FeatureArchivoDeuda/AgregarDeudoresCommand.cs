using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands.FeatureArchivoDeuda
{  /// <summary>
   /// Comando para agregar un nuevo deudor a la lista de archivo deuda.
   /// </summary>
    public class AgregarDeudoresCommand : IRequest<Guid>
    { 
        /// <summary>
        /// Obtiene o establece la información del deudor a agregar.
        /// </summary>
        public AgregaDeudoresRequest Deudor { get; set; }

        /// <summary>
        /// Crea una nueva instancia del comando AgregarDeudoresCommand.
        /// </summary>
        /// <param name="request">Información del deudor a agregar.</param>
        public AgregarDeudoresCommand(AgregaDeudoresRequest request)
        {
            Deudor = request;
        }
    }
}
