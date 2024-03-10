using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands
{  /// <summary>
   /// Comando para agregar un nuevo administrador.
   /// </summary>
    public class AgregarAdministradorCommand : IRequest<Guid>
    { 
        /// <summary>
        /// Obtiene o establece la información del administrador a agregar.
        /// </summary>
        public AdministradorRequest Administrador { get; set; }

        /// <summary>
        /// Crea una nueva instancia del comando AgregarAdministradorCommand.
        /// </summary>
        /// <param name="request">Información del administrador a agregar.</param>
        public AgregarAdministradorCommand(AdministradorRequest request)
        {
            Administrador = request;
        }
    }
}
