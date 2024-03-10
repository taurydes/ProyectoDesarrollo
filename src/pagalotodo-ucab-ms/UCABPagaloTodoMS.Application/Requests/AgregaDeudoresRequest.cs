using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Requests
{

    public class AgregaDeudoresRequest
    {   /// <summary>
        /// Representa la solicitud de Agregacion de un los deudores a la entidad archivo deuda.
        /// </summary>
        public int Cedula { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public decimal? Deuda { get; set; }
        public Guid ServicioPrestadoId { get; set; } //Referencia al servicio Prestado 
        public Guid ConsumidorId { get; set; } //Referencia al consumidor 

    }
}
