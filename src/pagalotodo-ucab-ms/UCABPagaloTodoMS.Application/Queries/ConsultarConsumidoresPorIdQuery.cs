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
    /// Consulta para obtener la información de un consumidor por su ID.
    /// </summary>
    public class ConsultarConsumidoresPorIdQuery : IRequest<ConsumidorResponse>
    {
        /// <summary>
        /// ID del consumidor.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarConsumidoresPorIdQuery.
        /// </summary>
        /// <param name="id">ID del consumidor.</param>
        public ConsultarConsumidoresPorIdQuery(Guid id)
        {
            Id = id;
        }
    }
 

}
