using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Requests
{
    public class ActualizaEstadosPagoRabbitRequest
    {
        /// <summary>
        /// lista dinamica con todos los campos que se suben en el archivo excel desde el front
        /// </summary>
        public List<dynamic>? Datos { get; set; }

        /// <summary>
        /// establece o obtiene la refeerencia del pago al que se le va a actualizar el estado
        /// </summary>
       
        public string? Referencia { get; set; }
        /// <summary>
        /// obtiene o establece el estado del pago (aprobado,rechazado) por defento pendiente
        /// </summary>
        public string? Estado { get; set; }

        
    }
}