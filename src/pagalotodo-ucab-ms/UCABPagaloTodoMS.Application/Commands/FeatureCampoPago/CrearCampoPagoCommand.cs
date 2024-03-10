using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;


namespace UCABPagaloTodoMS.Application.Commands.FeatureCampoPago
{
    /// <summary>
    /// Comando para agregar un nuevo campo.
    /// </summary>
    public class CrearCampoPagoCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece la información del campo a agregar.
        /// </summary>
        public CrearCampoPagoRequest CampoPago { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando CrearCampoPagoCommand.
        /// </summary>
        /// <param name="request">La información del campo a agregar.</param>
        public CrearCampoPagoCommand(CrearCampoPagoRequest request)
        {
            CampoPago = request;
        }
    }
}
