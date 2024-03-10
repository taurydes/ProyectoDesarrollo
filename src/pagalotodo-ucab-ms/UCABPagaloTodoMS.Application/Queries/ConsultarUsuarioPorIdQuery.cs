using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    /// <summary>
    /// Consulta para obtener un usuario por su identificador único.
    /// </summary>
    public class ConsultarUsuarioPorIdQuery : IRequest<UsuarioResponse>
    {
        /// <summary>
        /// Obtiene o establece el identificador único del usuario.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarUsuarioPorIdQuery.
        /// </summary>
        /// <param name="id">Identificador único del usuario.</param>
        public ConsultarUsuarioPorIdQuery(Guid id)
        {
            Id = id;
        }
    }
 

}
