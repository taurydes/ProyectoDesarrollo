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
    /// <summary>
    /// Consulta para obtener un usuario por su correo electrónico.
    /// </summary>
    [ExcludeFromCodeCoverage]

    public class ConsultarUsuarioPorCorreoQuery : IRequest<UsuarioResponse>
    {
        /// <summary>
        /// Obtiene o establece el correo electrónico del usuario.
        /// </summary>
        public string Correo { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarUsuarioPorCorreoQuery.
        /// </summary>
        /// <param name="correo">Correo electrónico del usuario.</param>
        public ConsultarUsuarioPorCorreoQuery(string correo)
        {
            Correo = correo;
        }
    }


}
