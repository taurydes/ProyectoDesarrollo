using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UCABPagaloTodoWeb.Models
{
    public class ServicioConfiguracionPagoModel
    {
        public Guid Id { get; set; }
        public Guid PrestadorServicioId { get; set; }
        public Guid ConfiguracionPagoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public float Costo { get; set; }
        public string EstadoServicio { get; set; }//(activo,inactivo,pronto)
        public bool TipoPago { get; set; } // <<-- revisar se acord� booleano 
        public bool EstatusServicio { get; set; }
        public List<CampoModel>? CamposConfiguracion { get; set; } // lista de pagos por servicios 

      



    }
}