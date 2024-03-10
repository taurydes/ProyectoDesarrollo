using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Commands.FeatureServiciosPrestados
{
    /// <summary>
    /// clase para agregar coficuación de pago
    /// </summary>
    public class AgregaConfiguracionCommand : IRequest<Guid>
    {
        /// <summary>
        /// obtiene o establece el servicio prestado al que pertenece la configuración
        /// </summary>
        public Guid ServicioId { get; set; }

        /// <summary>
        /// obtiene o establece el Id de la configuración de pago
        /// </summary>
        public Guid Configuracionid { get; set; }

        /// <summary>
        /// metodo constructor para la clase AgregaConfiguracionCommand
        /// </summary>
        /// <param name="servicioid"></param>
        /// <param name="configuracionid"></param>
        public AgregaConfiguracionCommand(Guid servicioid, Guid configuracionid)
        {
            ServicioId = servicioid;
            Configuracionid = configuracionid;

        }
    }
    
}
