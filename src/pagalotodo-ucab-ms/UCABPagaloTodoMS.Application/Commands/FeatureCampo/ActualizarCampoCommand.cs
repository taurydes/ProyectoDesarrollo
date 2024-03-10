using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Requests;

namespace UCABPagaloTodoMS.Application.Commands.FeatureCampo
    {
    /// <summary>
    /// Comando para actualizar un campo.
    /// </summary>
    public class ActualizarCampoCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece el ID del campo.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Obtiene o establece la información actualizada del campo.
        /// </summary>
        public CampoUpdateRequest Campo { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia del comando ActualizarCampoCommand.
        /// </summary>
        /// <param name="campo">La información actualizada del campo.</param>
        /// <param name="id">El ID del consumidor.</param>
        public ActualizarCampoCommand(CampoUpdateRequest campo, Guid id)
        {
            Campo = campo;
            Id = id;
        }
    }
}
