using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Requests
{
    public class CampoUpdateRequest
    {
        public Guid Id { get; set; } // agregado
        public string? NombreCampo { get; set; }
        public string? Tipo { get; set; }
        public int? Longitud { get; set; }
        public bool  Requerido { get; set; }
        
    }
}
