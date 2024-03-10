using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;

public class ConsultarUsuarioPorNombreQueryHandler : IRequestHandler<ConsultarUsuarioPorNombreQuery, UsuarioResponse>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<ConsultarUsuarioPorNombreQueryHandler> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de la clase ConsultarUsuarioPorNombreQueryHandler.
    /// </summary>
    /// <param name="dbContext">El contexto de la base de datos.</param>
    /// <param name="logger">El registrador.</param>
    public ConsultarUsuarioPorNombreQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarUsuarioPorNombreQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la consulta ConsultarUsuarioPorNombreQuery y devuelve la información de un usuario según su nombre de usuario.
    /// </summary>
    /// <param name="request">La consulta ConsultarUsuarioPorNombreQuery.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Un objeto UsuarioResponse que contiene la información del usuario.</returns>
    public async Task<UsuarioResponse> Handle(ConsultarUsuarioPorNombreQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                _logger.LogWarning("ConsultarUsuarioPorNombreQueryHandler.Handle: Request nulo.");
                throw new RequestNullException("El objeto request no puede ser nulo.");
            }
            else
            {
                return await HandleAsync(request.NombreUsuario);
            }
        }
        catch (Exception)
        {
            _logger.LogWarning("ConsultarUsuarioPorNombreQueryHandler.Handle: ArgumentNullException");
            throw;
        }
    }

    /// <summary>
    /// Recupera la información de un usuario según su nombre de usuario desde la base de datos.
    /// </summary>
    /// <param name="nombreUsuario">El nombre de usuario del usuario.</param>
    /// <returns>Un objeto UsuarioResponse que contiene la información del usuario.</returns>
    private async Task<UsuarioResponse> HandleAsync(string nombreUsuario)
    {
        try
        {
            _logger.LogInformation("ConsultarUsuarioPorNombreQueryHandler.HandleAsync");

            var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

            if (usuario is null)
            {
                throw new UsuarioNotFoundException($"No se encontró ningún usuario con el Nombre especificado: {nombreUsuario}");
            }

            if (usuario is Consumidor)
            {
                
               var consumidor = await _dbContext.Consumidores.Where(s => s.EstatusCuenta == true).FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
                    
               if (consumidor == null)
               {
                _logger.LogError("El Consumidor de Servicio está Inactivo");
                return new ConsumidorResponse();
               }
                var response = UsuarioMapper.MapEntityConsumidorAResponse(consumidor);

                return response;
            }
               
        
            if (usuario is PrestadorServicio)
            {
               var prestador = await _dbContext.PrestadorServicios.Where(s => s.EstatusCuenta == true).FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
                if (prestador==null)
                {
                 _logger.LogError("El Consumidor de Servicio está Inactivo");
                 return new ConsumidorResponse();
                }
                var response = UsuarioMapper.MapEntityPrestadorServicioAResponse(prestador);

                return response;

            }

            if (usuario is Administrador)
            {
                var administrador = await _dbContext.Administradores.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

                var response = UsuarioMapper.MapEntityAdministradorAResponse(administrador);

                return response;

            }

            var responseUsuario = UsuarioMapper.MapEntityAResponse(usuario);

            return responseUsuario;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ConsultarUsuarioPorNombreQueryHandler.HandleAsync. {Mensaje}", ex.Message);
            throw;
        }
    }
}