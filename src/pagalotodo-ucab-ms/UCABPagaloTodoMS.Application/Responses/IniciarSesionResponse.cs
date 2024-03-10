using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCABPagaloTodoMS.Application.Responses
{
    /// <summary>
    /// Clase que representa la respuesta al iniciar sesión.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class IniciarSesionResponse
    {
        private UsuarioResponse usuario;
        private string token;

        /// <summary>
        /// Crea una nueva instancia de la clase IniciarSesionResponse.
        /// </summary>
        /// <param name="usuario">El objeto UsuarioResponse correspondiente al usuario autenticado.</param>
        /// <param name="token">El token de autenticación generado para el usuario.</param>
        public IniciarSesionResponse(UsuarioResponse usuario, string token)
        {
            this.usuario = usuario;
            this.token = token;
        }
    }
}