using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Queries
{
    public class ConsultarConfiguracionPagoPorIdQuery : IRequest<ConfiguracionPagoResponse>
    {
        public Guid Id { get; set; }

        public ConsultarConfiguracionPagoPorIdQuery(Guid id)
        {
            Id = id;
        }
    }
 

}
