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
    /// Comando para actualizar un administrador.
    /// </summary>
    public class ActualizarAdministradorCommand : IRequest<Guid>
    {
        /// <summary>
        /// Obtiene o establece el ID del administrador.
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// Obtiene o establece la información actualizada del administrador.
        /// </summary>
        public AdministradorUpdateRequest Administrador { get; set; }

        /// <summary>
        /// Crea una nueva instancia del comando ActualizarAdministradorCommand.
        /// </summary>
        /// <param name="administrador">Información actualizada del administrador.</param>
        /// <param name="id">ID del administrador.</param>
        public ActualizarAdministradorCommand(AdministradorUpdateRequest usuario, Guid id)
        {
            Administrador = usuario;
            Id = id;
        }
    }
}
