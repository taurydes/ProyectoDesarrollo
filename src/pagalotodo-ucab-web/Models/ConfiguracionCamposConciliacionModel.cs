using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class ConfiguracionCamposConciliacionModel
    {
        public Guid ConfiguracionPagoId { get; set; }
        public Guid ServicioPrestadoId { get; set; }
        public List<CampoPagoModel>? Campos { get; set; }
        //public List<string>? CamposSeleccionados { get; set; }
    }
}
