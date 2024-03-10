using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Services
{
    /// <summary>
    /// interfaz del servicio de claves para el Json Token
    /// </summary>
    public interface IJwtService
    {
        string GenerateJwtToken(Usuario usuario,List<string> roles);
    }
}