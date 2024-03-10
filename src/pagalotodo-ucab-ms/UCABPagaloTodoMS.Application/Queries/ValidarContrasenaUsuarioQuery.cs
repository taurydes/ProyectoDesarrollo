using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    /// <summary>
    /// Consulta para validar una contraseña de usuario.
    /// </summary>
    public class ValidarContrasenaUsuarioQuery : IRequest<bool>
    {
        /// <summary>
        /// Contraseña a validar.
        /// </summary>
        public string Clave { get; }

        /// <summary>
        /// Contraseña hash utilizada para la comparación.
        /// </summary>
        public string ClaveHash { get; }

        /// <summary>
        /// Crea una nueva instancia de la consulta de validación de contraseña de usuario.
        /// </summary>
        /// <param name="contrasena">Contraseña a validar.</param>
        /// <param name="contrasenaHash">Contraseña hash utilizada para la comparación.</param>
        public ValidarContrasenaUsuarioQuery(string contrasena, string contrasenaHash)
        {
            Clave = contrasena;
            ClaveHash = contrasenaHash;
        }
    }
}