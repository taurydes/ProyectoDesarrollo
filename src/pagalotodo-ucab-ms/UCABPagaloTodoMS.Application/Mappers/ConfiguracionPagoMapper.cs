using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;
using static MassTransit.MessageHeaders;

namespace UCABPagaloTodoMS.Application.Mappers
{
    /// <summary>
    /// Clase estática que proporciona métodos para mapear entidades relacionadas con configuracion.
    /// </summary>
    [ExcludeFromCodeCoverage]

    public static class ConfiguracionPagoMapper
    {
        /// <summary>
        /// Mapea una entidad configuracion a un objeto ConfiguracionPagoResponse.
        /// </summary>
        /// <param name="entity">La entidad configuracion.</param>
        /// <returns>Un objeto ConfiguracionPagoResponse mapeado.</returns>
        public static ConfiguracionPagoResponse MapEntityAResponse(ConfiguracionPago entity)
        {
            var response = new ConfiguracionPagoResponse()
            {
                Id = entity.Id,
                ServicioPrestadoId = entity.ServicioPrestadoId,
                Campos =entity.Campos,
            };
            return response;
        }

        /// <summary>
        /// Mapea una solicitud CrearConfiguracionPagoRequest a una entidad configuracion.
        /// </summary>
        /// <param name="request">La solicitud CrearConfiguracionPagoRequest.</param>
        /// <returns>Una entidad configuracion mapeada.</returns>
        public static ConfiguracionPago MapRequestPagoEntity(CrearConfiguracionPagoRequest request)
        {
            var entity = new ConfiguracionPago()
            {
               
                ServicioPrestadoId = request.ServicioPrestadoId,
               
            };
            return entity;
        }
       
    }
}
