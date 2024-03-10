namespace UCABPagaloTodoMS.Application.Services
{
    /// <summary>
    /// interfaces del servicio de correos
    /// </summary>
    public interface IMailService
    {

        Task EnviarCorreoElectronicoAsync(string email, string asuntoMessage, string bodyMessage);
        
       
    }
}