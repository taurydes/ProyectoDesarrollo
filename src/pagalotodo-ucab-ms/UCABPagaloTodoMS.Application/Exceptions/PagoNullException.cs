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
    public class PagoNullException : Exception
    {
        /// <summary>
        /// contructor cuando se le para un mensaje
        /// </summary>
        /// <param name="message"></param>
        public PagoNullException(string message) : base(message)
        {
        }

    }
}
