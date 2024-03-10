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
    public class ActualizarContrasenaCommand : IRequest<bool>
    {
        /// <summary>
        /// Obtiene o establece la información ingresada por el usuario.
        /// </summary>
        public ContrasenaUpdateRequest DatosUsuario { get; set; }

        /// <summary>
        /// constructor de la clase, recibe un objeto ContrasenaUpdateRequest con los datos ingresados para cambiar
        /// </summary>
        /// <param name="usuario"></param>
        public ActualizarContrasenaCommand(ContrasenaUpdateRequest usuario)
        {
            DatosUsuario = usuario;
        }
    }
    
}
