using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class PrestadorModel : UsuarioModel

    {
        public string NombreEmpresa { get; set; }
        public string Rif { get; set; }

        public List<ServicioModel>? ServiciosPrestados { get; set; }
        public bool EstatusCuenta { get; set; }


    }
}