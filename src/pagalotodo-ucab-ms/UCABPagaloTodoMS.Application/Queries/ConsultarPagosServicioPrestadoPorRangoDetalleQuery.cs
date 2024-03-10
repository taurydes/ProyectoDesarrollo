using MediatR;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    /// <summary>
    /// Consulta para obtener un archivo de conciliación por rango de fechas.
    /// </summary>
    public class ConsultarPagosServicioPrestadoPorRangoDetalleQuery : IRequest<ServicioPrestadoPagosRangoResponse>
    {


        public PagosServicioPrestadoPorRangoDetalleRequest DatosServicio { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarPagosServicioPrestadoPorRangoDetalleQuery.
        /// </summary>
        /// <param name="_request"> instancia de PagosServicioPrestadoPorRangoDetalleRequest donde se obtienen los datos del servicio(Id,FechaInicio,FechaFin,CamposRequeridos).</param>
        public ConsultarPagosServicioPrestadoPorRangoDetalleQuery(PagosServicioPrestadoPorRangoDetalleRequest _request)
        {
            DatosServicio = _request;
        }
    }
}