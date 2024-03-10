using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class PagoModel
    {
        public Guid Id { get; set; }

        public Guid ConsumidorId { get; set; }

        public Guid ServicioPrestadoId { get; set; }

        public string? Referencia { get; set; }

        public bool EstatusPago { get; set; } = true;

        public List<CampoPagoModel>? CamposPago { get; set; }


    }
}
