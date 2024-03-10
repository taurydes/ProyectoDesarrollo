using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class LoginViewModel

    {
        public Guid Id { get; set; }
        public string NombreUsuario { get; set; } // revisar, tengo que quitarlo
        public string Clave { get; set; }
       
    }
}
