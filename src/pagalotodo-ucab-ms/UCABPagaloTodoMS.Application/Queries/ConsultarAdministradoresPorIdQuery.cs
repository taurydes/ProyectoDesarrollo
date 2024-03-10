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
    /// Consulta para obtener un administrador por su ID.
    /// </summary>
    public class ConsultarAdministradoresPorIdQuery : IRequest<AdministradorResponse>
    {
        /// <summary>
        /// Obtiene o establece el ID del administrador.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConsultarAdministradoresPorIdQuery"/>.
        /// </summary>
        /// <param name="id">ID del administrador.</param>
        public ConsultarAdministradoresPorIdQuery(Guid id)
        {
            Id = id;
        }
    }
 

}
