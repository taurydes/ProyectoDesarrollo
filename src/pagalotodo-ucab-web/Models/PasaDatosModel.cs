using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class PasaDatosModel 
    {
        public Guid PrestadorServicioId { get; set; }
        public Guid ConsumidorId { get; set; }
        public Guid ServicioId { get; set; }
         

    }
}