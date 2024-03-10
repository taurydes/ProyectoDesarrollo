using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class UsuarioModel
    {
        public Guid Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Clave { get; set; }
        public string Correo { get; set; }
        


    }
}