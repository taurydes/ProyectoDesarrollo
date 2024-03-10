using System.Diagnostics.CodeAnalysis;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;
using static MassTransit.MessageHeaders;

namespace UCABPagaloTodoMS.Application.Mappers
{
    /// <summary>
    /// Clase estática que proporciona métodos para mapear entidades relacionadas con pagos.
    /// </summary>
    //[ExcludeFromCodeCoverage]

    public static class PagoMapper
    {


        /// <summary>
        /// Mapea una entidad Pago a un objeto PagoResponse.
        /// </summary>
        /// <param name="entity">La entidad Pago.</param>
        /// <returns>Un objeto PagoResponse mapeado.</returns>
        public static PagoResponse MapEntityAResponse(Pago entity)
        {
            var response = new PagoResponse()
            {

                Id = entity.Id,
                ServicioPrestadoId = entity.ServicioPrestadoId,
                ConsumidorId = entity.ConsumidorId,
                EstadoPago = entity.EstadoPago,
                Referencia = entity.Referencia,
                CamposPago = entity.CamposPago,
            };
            return response;
        }


        /// <summary>
        /// Mapea una solicitud CrearPagoRequest a una entidad Pago.
        /// </summary>
        /// <param name="request">La solicitud CrearPagoRequest.</param>
        /// <returns>Una entidad Pago mapeada.</returns>
        public static Pago MapRequestPagoEntity(CrearPagoRequest request)
        {
            var entity = new Pago()
            {
               
                ServicioPrestadoId = request.ServicioPrestadoId,
                ConsumidorId = request.ConsumidorId,
                Referencia = request.Referencia,
            };
            return entity;
        }
       
    }
}
