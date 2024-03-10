using MediatR;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    /// <summary>
    /// Consulta para obtener un archivo de conciliación por rango de fechas.
    /// </summary>
    public class ConsultarPagosAprobadoQuery : IRequest<ServicioPrestadoPagosRangoResponse>
    {


        public PagosServicioPrestadoPorRangoDetalleRequest DatosServicio { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ConsultarPagosAprobadoQuery.
        /// </summary>
        /// <param name="_request"> instancia de PagosServicioPrestadoPorRangoDetalleRequest donde se obtienen los datos del servicio(Id,FechaInicio,FechaFin,CamposRequeridos).</param>
        public ConsultarPagosAprobadoQuery(PagosServicioPrestadoPorRangoDetalleRequest _request)
        {
            DatosServicio = _request;
        }
    }
}