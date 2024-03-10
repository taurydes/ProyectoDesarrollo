using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;
using static MassTransit.MessageHeaders;

namespace UCABPagaloTodoMS.Application.Mappers
{
    /// <summary>
    /// Clase estática que proporciona métodos para mapear entidades relacionadas con servicios prestados.
    /// </summary>
    //[ExcludeFromCodeCoverage]

    public static class ServicioPrestadoMapper
    {
        /// <summary>
        /// Mapea una entidad ServicioPrestado a un objeto ServicioPrestadoResponse.
        /// </summary>
        /// <param name="entity">La entidad ServicioPrestado.</param>
        /// <returns>Un objeto ServicioPrestadoResponse mapeado.</returns>
        public static ServicioPrestadoResponse MapEntityAResponse(ServicioPrestado entity)
        {
            var response = new ServicioPrestadoResponse()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Descripcion= entity.Descripcion,
                Costo= entity.Costo,
                EstadoServicio= entity.EstadoServicio,
                TipoPago= entity.TipoPago,
                PagosPorServicioRealizados =entity.PagosPorServicioRealizados,
                ConfiguracionPagoId = entity.ConfiguracionPagoId,
                PrestadorServicioId = entity.PrestadorServicioId,
                ArchivosPertenecientes = entity.ArchivosPertenecientes,
                EstatusServicio = entity.EstatusServicio,

            };
            return response;
        }

        /// <summary>
        /// Mapea una entidad ServicioPrestado a un objeto ServicioPrestadoPagosRangoResponse.
        /// </summary>
        /// <param name="entity">La entidad ServicioPrestado.</param>
        /// <returns>Un objeto ServicioPrestadoPagosRangoResponse mapeado.</returns>
        public static ServicioPrestadoPagosRangoResponse MapEntityServicioRangoResponse(ServicioPrestado entity,string _nombreEmpresa, string _correo)
        {
            var response = new ServicioPrestadoPagosRangoResponse()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                NombreEmpresa = _nombreEmpresa,
                PagosPorServicioRealizados = entity.PagosPorServicioRealizados,
                Correo = _correo,
            };
            return response;
        }

        /// <summary>
        /// Mapea una solicitud CrearServicioPrestadoRequest a una entidad ServicioPrestado.
        /// </summary>
        /// <param name="request">La solicitud CrearServicioPrestadoRequest.</param>
        /// <returns>Una entidad ServicioPrestado mapeada.</returns>
        public static ServicioPrestado MapRequestServicioPrestadoEntity(CrearServicioPrestadoRequest request)
        {
            var entity = new ServicioPrestado()
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Costo = request.Costo,
                EstadoServicio = request.EstadoServicio,
                TipoPago = request.TipoPago,
                PrestadorServicioId = request.PrestadorServicioId,
                

            };
            return entity;
        }

    }
}
