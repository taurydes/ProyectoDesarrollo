using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;

/// <summary>
/// Maneja la consulta ConsultarUsuarioPorCorreoQuery al recuperar información del usuario desde la base de datos.
/// </summary>
public class ConsultarUsuarioPorCorreoQueryHandler : IRequestHandler<ConsultarUsuarioPorCorreoQuery, UsuarioResponse>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<ConsultarUsuarioPorCorreoQueryHandler> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de la clase ConsultarUsuarioPorCorreoQueryHandler.
    /// </summary>
    /// <param name="dbContext">El contexto de la base de datos.</param>
    /// <param name="logger">El registrador.</param>
    public ConsultarUsuarioPorCorreoQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarUsuarioPorCorreoQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la consulta ConsultarUsuarioPorCorreoQuery y devuelve la información del usuario.
    /// </summary>
    /// <param name="request">La consulta ConsultarUsuarioPorCorreoQuery.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>El UsuarioResponse que contiene la información del usuario.</returns>
    public async Task<UsuarioResponse> Handle(ConsultarUsuarioPorCorreoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                _logger.LogWarning("ConsultarUsuarioPorCorreoQueryHandler.Handle: Request nulo.");
                throw new RequestNullException("El Request es nulo");
            }
            if (string.IsNullOrEmpty(request.Correo))
            {
                throw new CorreoElectronicoNullException("El correo electrónico no puede ser nulo o vacío.");
            }
            else
            {
                return await HandleAsync(request.Correo);
            }
        }
        catch (UsuarioNotFoundException ex)
        {
            _logger.LogError(ex, "UsuarioNotFoundException en ConsultarUsuarioPorCorreoQueryHandler.Handle");
            throw;
        }
        catch (CorreoElectronicoNullException ex)
        {
            _logger.LogError(ex, "CorreoElectronicoNullException en ConsultarUsuarioPorCorreoQueryHandler.Handle");
            throw;
        }
        catch (RequestNullException ex)
        {
            _logger.LogWarning("El Request es nulo");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("ConsultarUsuarioPorCorreoQueryHandler.Handle: ArgumentNullException");
            throw;
        }
    }

    /// <summary>
    /// Recupera la información del usuario desde la base de datos en función del correo electrónico proporcionado.
    /// </summary>
    /// <param name="correoElectronico">El correo electrónico del usuario a recuperar.</param>
    /// <returns>El UsuarioResponse que contiene la información del usuario.</returns>
    private async Task<UsuarioResponse> HandleAsync(string correoElectronico)
    {
        try
        {
            _logger.LogInformation("ConsultarUsuarioPorCorreoQueryHandler.HandleAsync");

            var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Correo == correoElectronico);

            if (usuario is null)
            {
                throw new UsuarioNotFoundException("No se encontró ningún usuario con el correo electrónico proporcionado.");
            }

            var response = UsuarioMapper.MapEntityAResponse(usuario);

            return response;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ConsultarUsuarioPorCorreoQueryHandler.HandleAsync. {Mensaje}", ex.Message);
            throw;
        }
    }
}
