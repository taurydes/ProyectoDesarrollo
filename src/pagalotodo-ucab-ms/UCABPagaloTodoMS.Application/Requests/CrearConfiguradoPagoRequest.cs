using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Requests
{
    public class CrearConfiguracionPagoRequest
    {
        /// <summary>
        /// Obtiene o establece el ID del servicio al cual pertenece esa configuracion de pago.
        /// </summary>
        public Guid ServicioPrestadoId { get; set; }
        

    }
}
