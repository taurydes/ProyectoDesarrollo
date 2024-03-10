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
    public class CorreoElectronicoNullException : Exception
    {
        /// <summary>
        /// contructor cuando se le para un mensaje y se le pasa al padre
        /// </summary>
        /// <param name="message"></param>
        public CorreoElectronicoNullException(string message) : base(message)
        {
        }

    }
}
