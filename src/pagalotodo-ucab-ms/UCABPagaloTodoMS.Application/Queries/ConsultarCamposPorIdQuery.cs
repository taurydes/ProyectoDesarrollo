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
    /// Consulta para obtener la información de un campo por su ID.
    /// </summary>
    public class ConsultarCamposPorIdQuery : IRequest<CampoResponse>
    {
        /// <summary>
        /// ID del campo.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarCamposPorIdQuery.
        /// </summary>
        /// <param name="id">ID del campo.</param>
        public ConsultarCamposPorIdQuery(Guid id)
        {
            Id = id;
        }
    }
 

}
