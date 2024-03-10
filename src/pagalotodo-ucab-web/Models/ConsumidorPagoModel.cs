using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class ConsumidorPagoModel : UsuarioModel

    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int? Telefono { get; set; }
        public string? Direccion { get; set; }
        public bool EstatusCuenta { get; set; }
        public List<PagoModel>? PagosRealizados { get; set; }

        //public List<ArchivoDeuda>? PagosPendientes { get; set; }
        public string? NombreServicio { get; set; }// probando
    }
}