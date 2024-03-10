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
    /// Consulta para obtener un prestador de servicio por su Id.
    /// </summary>
    public class ConsultarPrestadoresPorIdQuery : IRequest<PrestadorServicioResponse>
    {
        /// <summary>
        /// Obtiene o establece el Id del prestador de servicio.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarPrestadoresPorIdQuery.
        /// </summary>
        /// <param name="id">Id del prestador de servicio.</param>
        public ConsultarPrestadoresPorIdQuery(Guid id)
        {
            Id = id;
        }
    }
 

}
