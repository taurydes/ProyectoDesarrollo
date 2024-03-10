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
    /// clase responde de el tipo campo
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CampoResponse
    {
        public Guid Id { get; set; }
        public Guid ConfiguracionPagoId { get; set; }
        public string? NombreCampo { get; set; }
        public string? Tipo { get; set; }
        public int? Longitud { get; set; }
        public bool Requerido { get; set; }

    }
}
