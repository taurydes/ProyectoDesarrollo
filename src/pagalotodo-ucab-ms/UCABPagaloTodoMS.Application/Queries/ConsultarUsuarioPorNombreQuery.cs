using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    [ExcludeFromCodeCoverage]
    /// <summary>
    /// Consulta para obtener un usuario por su nombre de usuario.
    /// </summary>
    public class ConsultarUsuarioPorNombreQuery : IRequest<UsuarioResponse>
    {
        /// <summary>
        /// Obtiene o establece el nombre de usuario.
        /// </summary>
        public string NombreUsuario { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarUsuarioPorNombreQuery.
        /// </summary>
        /// <param name="nombreUsuario">Nombre de usuario.</param>
        public ConsultarUsuarioPorNombreQuery(string nombreUsuario)
        {
            NombreUsuario = nombreUsuario;
        }
    }
}
