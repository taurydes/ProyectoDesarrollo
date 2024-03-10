using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class CampoModel

    {
        public Guid Id { get; set; }
        public string? NombreCampo { get; set; }
        public string? Tipo { get; set; }
        public int? Longitud { get; set; }
        public bool Requerido { get; set; }
        public Guid ConfiguracionPagoId { get; set; } //Referencia a la configuración 

        public Guid ServicioPrestadoId { get; set; } //Referencia al servicio Prestado 



    }
}
