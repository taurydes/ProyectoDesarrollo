using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class CampoPagoModel
    {
        public Guid Id { get; set; }
        public Guid PagoId { get; set; } //Referencia al PagoID
        public string? NombreCampo { get; set; }
        public string? Tipo { get; set; }
        public int? Longitud { get; set; }
        public string? Valor { get; set; }


    }
}
