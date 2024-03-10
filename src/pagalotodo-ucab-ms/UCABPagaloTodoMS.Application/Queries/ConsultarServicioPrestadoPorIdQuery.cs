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
    /// Consulta para obtener un servicio prestado por su ID.
    /// </summary>
    public class ConsultarServicioPrestadoPorIdQuery : IRequest<ServicioPrestadoResponse>
    {
        /// <summary>
        /// Obtiene o establece el ID del servicio prestado.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarServicioPrestadoPorIdQuery.
        /// </summary>
        /// <param name="id">ID del servicio prestado a consultar.</param>
        public ConsultarServicioPrestadoPorIdQuery(Guid id)
        {
            Id = id;
        }
    }
 

}
