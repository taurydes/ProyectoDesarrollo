using System.Diagnostics.CodeAnalysis;
using System.Security.Policy;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;
using UCABPagaloTodoMS.Infrastructure.Settings;
using static MassTransit.MessageHeaders;

namespace UCABPagaloTodoMS.Application.Mappers
{
    /// <summary>
    /// Clase estática que proporciona métodos para mapear entidades relacionadas con campos.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class CampoPagoMapper
    {
        /// <summary>
        /// Mapea una entidad campo a un objeto CampoPagoResponse.
        /// </summary>
        /// <param name="entity">La entidad campo.</param>
        /// <returns>Un objeto CampoPagoResponse mapeado.</returns>
        public static CampoPagoResponse MapEntityAResponse(CampoPago entity)
        {
            var response = new CampoPagoResponse()
            {
                Id = entity.Id,
                PagoId = entity.PagoId,
                NombreCampo = entity.NombreCampo,
                Tipo = entity.Tipo,
                Longitud = entity.Longitud,
                Valor = entity.Valor,
            };
            return response;
        }

        /// <summary>
        /// Mapea una solicitud CrearCampoPagoRequest a una entidad campo.
        /// </summary>
        /// <param name="request">La solicitud CrearCampoPagoRequest.</param>
        /// <returns>Una entidad campo mapeada.</returns>
        public static CampoPago MapRequestCampoEntity(CrearCampoPagoRequest request)
        {
            var entity = new CampoPago()
            {
                PagoId = request.PagoId,
                NombreCampo =request.NombreCampo,
                Tipo = request.Tipo,
                Longitud = request.Longitud,
                Valor = request.Valor,
                
            };
            return entity;
        }
      

       
    }
}
