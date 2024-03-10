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
    /// Consulta para obtener la información de un archivo por su ID.
    /// </summary>
    public class ConsultarArchivoPorIdQuery : IRequest<ArchivoConciliacionResponse>
    {
        /// <summary>
        /// ID del archivo.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarArchivoPorIdQuery.
        /// </summary>
        /// <param name="id">ID del archivo.</param>
        public ConsultarArchivoPorIdQuery(Guid id)
        {
            Id = id;
        }
    }
 

}
