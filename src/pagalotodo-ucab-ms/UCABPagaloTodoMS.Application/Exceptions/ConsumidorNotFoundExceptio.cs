using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Exceptions
{
    /// <summary>
    /// clase tipo exepción personalizada 
    /// </summary>
    public class ConsumidorNotFoundException : Exception
    {
        /// <summary>
        /// contructor que usa el metodo constructor del padre que recibe un mensaje
        /// </summary>
        public ConsumidorNotFoundException(Guid consumidorId) : base($"No se encontró el consumidor con el ID: {consumidorId}")
        {
        }
    }
}
