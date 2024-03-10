using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace UCABPagaloTodoMS.Application.Services
{
    /// <summary>
    /// clase para manejar los errores en un servicio
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ErrorResponseService
    {
        public int ErrorCode { get; set; }
        public List<string>? ErrorMessages { get; set; }
    }
}


