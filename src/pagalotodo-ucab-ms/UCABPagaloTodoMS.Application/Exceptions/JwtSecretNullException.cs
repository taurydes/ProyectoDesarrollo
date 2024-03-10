﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Exceptions
{
    /// <summary>
    /// clase tipo exepción personalizada 
    /// </summary>
    public class JwtSecretNullException : Exception
    {
        /// <summary>
        /// contructor base, sin parametros
        /// </summary>
        public JwtSecretNullException() { }

        /// <summary>
        /// contructor cuando se le para un mensaje
        /// </summary>
        /// <param name="message"></param>
        public JwtSecretNullException(string message) : base(message) { }


        /// <summary>
        /// contructor que recibe el mensaje y la información de la excepción 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public JwtSecretNullException(string message, Exception innerException) : base(message, innerException) { }
    }
}
