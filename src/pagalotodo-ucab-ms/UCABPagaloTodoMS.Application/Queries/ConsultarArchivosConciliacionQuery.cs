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
    /// Consulta para obtener la lista de consumidores.
    /// </summary>
    public class ConsultarArchivosConciliacionQuery : IRequest<List<ArchivoConciliacionResponse>>
    {
        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarArchivosConciliacionQuery.
        /// </summary>
        public ConsultarArchivosConciliacionQuery()
        {
        }
}

}
