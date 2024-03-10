using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    /// <summary>
    /// Consulta para obtener valor booleano si un consumidor se encuentra en un ArchvoDeuda.
    /// </summary>
    [ExcludeFromCodeCoverage]

    public class ConsultarConsumidorArchivoDeudaQuery : IRequest<bool>
    {
        /// <summary>
        /// Obtiene o establece los datos requeridos para ls consulta del Archivo.
        /// </summary>
       public ConsumidorArchivoDeudaRequest ConsultaArchivo { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarConsumidorArchivoDeudaQuery para la consulta de si un consumdior tiene una deuda con un servicio.
        /// </summary>
        /// <param name="_consultaArchivo">request requerido para la consulta, que contiene los  Id del servicio y el consumidor.</param>
        public ConsultarConsumidorArchivoDeudaQuery(ConsumidorArchivoDeudaRequest _consultaArchivo)
        {
            ConsultaArchivo = _consultaArchivo;
           
        }
    }


}
