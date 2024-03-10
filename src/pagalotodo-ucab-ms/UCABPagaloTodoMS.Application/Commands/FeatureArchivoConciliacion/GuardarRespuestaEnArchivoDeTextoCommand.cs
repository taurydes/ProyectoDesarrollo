using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;

namespace UCABPagaloTodoMS.Application.Commands.FeatureArchivoConciliacion
{  /// <summary>
   /// Comando para genear el archivo de conciliación en un csv.
   /// </summary>
    public class GuardarRespuestaEnArchivoDeTextoCommand : IRequest<string>
    {
        public ServicioPrestadoPagosRangoResponse Response { get; }

        public GuardarRespuestaEnArchivoDeTextoCommand(ServicioPrestadoPagosRangoResponse response)
        {
            Response = response;
        }
    }
}
