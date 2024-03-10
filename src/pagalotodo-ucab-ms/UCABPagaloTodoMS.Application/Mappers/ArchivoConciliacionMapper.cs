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
    public static class ArchivoConciliacionMapper
    {
        /// <summary>
        /// Mapea una solicitud AgregarArchivoConciliacionRequest a una entidad campo.
        /// </summary>
        /// <param name="request">La solicitud AgregarArchivoConciliacionRequest.</param>
        /// <returns>Una entidad archivo mapeada.</returns>
        public static ArchivoConciliacion MapRequestConciliacionEntity(AgregarArchivoConciliacionRequest request)
        {
            var entity = new ArchivoConciliacion()
            {
                ServicioPrestadoId = request.ServicioPrestadoId,
                UrlDescarga = request.UrlDescarga,
            };
            return entity;
        }

        public static ArchivoConciliacionResponse MapEntityResponse(ArchivoConciliacion entity)
        {
            var response = new ArchivoConciliacionResponse()
            {
                Id = entity.Id,
                ServicioPrestadoId = entity.ServicioPrestadoId,
                Confirmado = entity.Confirmado,
                UrlDescarga = entity.UrlDescarga
            };
            return response;
        }

    }
}
