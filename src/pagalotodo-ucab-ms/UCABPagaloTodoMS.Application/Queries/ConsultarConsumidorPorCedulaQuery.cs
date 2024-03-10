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
    /// Consulta para obtener un usuario por su cedula.
    /// </summary>
    [ExcludeFromCodeCoverage]

    public class ConsultarConsumidorPorCedulaQuery : IRequest<ConsumidorResponse>
    {
        /// <summary>
        /// Obtiene o establece el cedula del usuario.
        /// </summary>
        public int Cedula { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarConsumidorPorCedulaQuery.
        /// </summary>
        /// <param name="cedula">cedula del usuario.</param>
        public ConsultarConsumidorPorCedulaQuery(int cedula)
        {
            Cedula = cedula;
        }
    }


}
