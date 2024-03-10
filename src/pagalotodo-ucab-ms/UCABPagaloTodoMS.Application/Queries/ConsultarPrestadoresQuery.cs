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
    /// Consulta para obtener una lista de prestadores de servicio.
    /// </summary>
    public class ConsultarPrestadoresQuery : IRequest<List<PrestadorServicioResponse>>
    {
        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarPrestadoresQuery.
        /// </summary>
        public ConsultarPrestadoresQuery()
        {
        }
    }

}
