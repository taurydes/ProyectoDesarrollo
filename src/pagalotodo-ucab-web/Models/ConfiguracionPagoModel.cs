using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class ConfiguracionPagoModel

    {
        public Guid ConfiguracionPagoId { get; set; }
        public Guid ConsumidorId { get; set; }
        public Guid ServicioPrestadoId { get; set; }
        public List<CampoModel>? Campos { get; set; }

        public bool TipoPago { get; set; }
        public bool EstatusServicio { get; set; }


    }
}
