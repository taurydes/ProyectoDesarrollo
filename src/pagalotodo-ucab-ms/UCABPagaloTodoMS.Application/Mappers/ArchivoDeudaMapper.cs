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
    /// Clase estática que proporciona métodos para mapear entidades relacionadas con archivos de deudas.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ArchivoDeudaMapper
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
        /// Mapea una solicitud AgregaDeudoresRequest a una entidad campo.
        /// </summary>
        /// <param name="request">La solicitud AgregaDeudoresRequest.</param>
        /// <returns>Una entidad campo mapeada.</returns>
        public static ArchivoDeuda MapRequestDeudorEntity(AgregaDeudoresRequest request)
        {
            var entity = new ArchivoDeuda()
            {
                ServicioPrestadoId = request.ServicioPrestadoId,
                ConsumidorId = request.ConsumidorId,
            };
            return entity;
        }
      

       
    }
}
