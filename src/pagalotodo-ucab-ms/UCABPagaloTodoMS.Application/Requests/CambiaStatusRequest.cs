namespace UCABPagaloTodoMS.Application.Requests
{
    /// <summary>
    /// Representa la solicitud de cambio de estatus de un consumidor.
    /// </summary>
    public class CambiaEstatusRequest
    {
        /// <summary>
        /// Obtiene o establece el identificador del consumidor.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nuevo estatus de la cuenta del consumidor.
        /// </summary>
        public bool EstatusCuenta { get; set; }
    }
}