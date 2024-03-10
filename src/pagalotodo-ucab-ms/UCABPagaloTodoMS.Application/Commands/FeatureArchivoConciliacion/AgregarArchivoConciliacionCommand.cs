using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands
{  /// <summary>
   /// Comando para agregar un nuevo archivo de conciliacion Aprobado.
   /// </summary>
    public class AgregarArchivoConciliacionCommand : IRequest<Guid>
    { 
        /// <summary>
        /// Obtiene o establece la información del archivo a agregar.
        /// </summary>
        public AgregarArchivoConciliacionRequest ArchivoRequest { get; set; }

        /// <summary>
        /// Crea una nueva instancia del comando AgregarArchivoConciliacionCommand.
        /// </summary>
        /// <param name="request">Información del archivo a agregar.</param>
        public AgregarArchivoConciliacionCommand(AgregarArchivoConciliacionRequest request)
        {
            ArchivoRequest = request;
        }
    }
}
