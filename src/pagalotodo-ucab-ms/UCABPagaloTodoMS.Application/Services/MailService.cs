using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace UCABPagaloTodoMS.Application.Services
{
    [ExcludeFromCodeCoverage]
  
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// constructor de la clase, recibe un objeto IConfiguration
        /// </summary>
        /// <param name="configuration"></param>
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Método asyncrono que recibe una dirección de correo y un codigo, los datos para el envío los obtiene de IConfiguration, en el AppSetting 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="asuntoMessage"></param>
        /// <param name="bodyMessage"></param>
        /// <returns></returns>
        public async Task EnviarCorreoElectronicoAsync(string email, string asuntoMessage, string bodyMessage)
        {
            var smtpConfig = _configuration.GetSection("SmtpConfig");

            var smtpServer = smtpConfig["SmtpServer"];
            var smtpPort = int.Parse(smtpConfig["SmtpPort"]);
            var smtpUsername = smtpConfig["SmtpUsername"];
            var smtpPassword = smtpConfig["SmtpPassword"];

            var smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
            };

            var message = new MailMessage
            {
                From = new MailAddress(smtpUsername),
                Subject = asuntoMessage,
                Body = bodyMessage,
            };
           
            message.To.Add(email);
            await smtpClient.SendMailAsync(message);
        }
                
    }
}


