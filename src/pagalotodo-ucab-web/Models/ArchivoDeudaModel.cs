using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class ArchivoDeudaModel
    {
        public int Cedula { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public decimal? Deuda { get; set; }
        public Guid ServicioPrestadoId { get; set; } //Referencia al servicio Prestado 
        public Guid ConsumidorId { get; set; } //Referencia al consumidor 

    }
}
