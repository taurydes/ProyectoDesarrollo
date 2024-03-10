using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Responses
{
    /// <summary>
    /// clase responses para los campos perteneciente a un pago
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CampoPagoResponse
    {
        public Guid Id { get; set; }
        public Guid PagoId { get; set; }
        public string? NombreCampo { get; set; }
        public string? Tipo { get; set; }
        public int? Longitud { get; set; }
        public string? Valor { get; set; }

    }
}
