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
    /// Comando para recuperar contraseña de un usuario.
    /// </summary>
    public class RecuperaContrasenaCommand : IRequest<string>
    {
        public RecuperaContraRequest Usuario { get; set; }

        public RecuperaContrasenaCommand(RecuperaContraRequest request)
        {
            Usuario = request;
        }
    }
}
