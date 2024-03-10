using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands.FeaturePrestadorServicio
    {
    /// <summary>
    /// Comando para actualizar un prestador de servicio.
    /// </summary>
    public class ActualizarPrestadorServicioCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece el identificador del prestador de servicio.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Obtiene o establece la información del prestador de servicio a actualizar.
        /// </summary>
        public PrestadorServicioUpdateRequest Prestador { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando ActualizarPrestadorServicioCommand.
        /// </summary>
        /// <param name="usuario">La información del prestador de servicio a actualizar.</param>
        /// <param name="id">El identificador del prestador de servicio.</param>
        public ActualizarPrestadorServicioCommand(PrestadorServicioUpdateRequest usuario, Guid id)
        {
            Prestador = usuario;
            Id = id;
        }
    }
}
