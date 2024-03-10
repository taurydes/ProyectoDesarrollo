using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;


namespace UCABPagaloTodoMS.Application.Commands.FeatureConfiguracionPago
{
    /// <summary>
    /// Comando para agregar un nuevo campo.
    /// </summary>
    public class CrearConfiguracionPagoCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece la información de la configuracion a agregar.
        /// </summary>
        public CrearConfiguracionPagoRequest ConfiguracionPago { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando CrearConfiguracionPagoCommand.
        /// </summary>
        /// <param name="request">La información de la configuracion pago a agregar.</param>
        public CrearConfiguracionPagoCommand(CrearConfiguracionPagoRequest request)
        {
            ConfiguracionPago = request;
        }
    }
}
