using System.Diagnostics.CodeAnalysis;
using System.Security.Policy;
using UCABPagaloTodoMS.Application.Commands.FeatureCampo;
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
    public static class CampoMapper
    {
        /// <summary>
        /// Mapea una entidad campo a un objeto CampoResponse.
        /// </summary>
        /// <param name="entity">La entidad campo.</param>
        /// <returns>Un objeto CampoResponse mapeado.</returns>
        public static CampoResponse MapEntityAResponse(Campo entity)
        {
            var response = new CampoResponse()
            {
                Id = entity.Id,
                ConfiguracionPagoId = entity.ConfiguracionPagoId,
                NombreCampo = entity.NombreCampo,
                Tipo = entity.Tipo,
                Longitud = entity.Longitud,
                Requerido = entity.Requerido,
            };
            return response;
        }


        /// <summary>
        /// Método estatico que mapea una entidad campo a un objeto ActualizarCampoCommand.
        /// </summary>
        /// <param name="entity">La entidad campo.</param>
        public static void MapEntityUpdateResponse(Campo entity, ActualizarCampoCommand request)
        {
            entity.NombreCampo = request.Campo.NombreCampo;
            entity.Tipo = request.Campo.Tipo;
            entity.Longitud = request.Campo.Longitud;
            entity.Requerido = request.Campo.Requerido;
                       
        }

        /// <summary>
        /// Mapea una solicitud CrearCampoRequest a una entidad campo.
        /// </summary>
        /// <param name="request">La solicitud CrearCampoRequest.</param>
        /// <returns>Una entidad campo mapeada.</returns>
        public static Campo MapRequestCampoEntity(CrearCampoRequest request)
        {
            var entity = new Campo()
            {
                ConfiguracionPagoId = request.ConfiguracionPagoId,
                NombreCampo = request.NombreCampo,
                Tipo = request.Tipo,
                Longitud = request.Longitud,
                Requerido = request.Requerido,
                
            };
            return entity;
        }
      

       
    }
}
