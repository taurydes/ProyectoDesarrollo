using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Requests
{

    public class AgregaConfiguracionPagoRequest
    {   /// <summary>
        /// Representa la solicitud de creación o actualización de un ConfiguracionPago.
        /// </summary>
        public Guid ServicioPrestadoId { get; set; }
        public Guid ConfiguracionPagoId { get; set; }
    }
}
