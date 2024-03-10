using System;
using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Core.Entities
{
    /// <summary>
    /// Clase base para todas las entidades en el sistema.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BaseEntity
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la entidad.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora de creación de la entidad.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Obtiene o establece el usuario que creó la entidad.
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora de la última actualización de la entidad.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Obtiene o establece el usuario que realizó la última actualización de la entidad.
        /// </summary>
        public string? UpdatedBy { get; set; }
    }
}