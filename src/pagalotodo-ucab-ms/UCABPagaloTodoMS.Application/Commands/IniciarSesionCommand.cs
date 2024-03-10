using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands
{
    [ExcludeFromCodeCoverage]

    /// <summary>
    /// Comando para iniciar sesión de un usuario.
    /// </summary>
    public class IniciarSesionCommand : IRequest<string>
    {
        /// <summary>
        /// Obtiene o establece la información del usuario para iniciar sesión.
        /// </summary>
        public UsuarioRequest Usuario { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando IniciarSesionCommand.
        /// </summary>
        /// <param name="request">La información del usuario para iniciar sesión.</param>
        public IniciarSesionCommand(UsuarioRequest request)
        {
            Usuario = request;
        }
    }
}
