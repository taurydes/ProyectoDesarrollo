using MediatR;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    /// <summary>
    /// Consulta para obtener un archivo de conciliación por rango de fechas.
    /// </summary>
    public class ConsultarPagosServicioPrestadoPorRangoQuery : IRequest<ServicioPrestadoPagosRangoResponse>
    {


        public PagosServicioPrestadoPorRangoRequest DatosServicio { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarPagosServicioPrestadoPorRangoQuery.
        /// </summary>
        /// <param name="_request"> instancia de PagosServicioPrestadoPorRangoRequest donde se obtienen los datos del servicio(Id,FechaInicio,FechaFin).</param>
        public ConsultarPagosServicioPrestadoPorRangoQuery(PagosServicioPrestadoPorRangoRequest _request)
        {
            DatosServicio = _request;
        }
    }
}