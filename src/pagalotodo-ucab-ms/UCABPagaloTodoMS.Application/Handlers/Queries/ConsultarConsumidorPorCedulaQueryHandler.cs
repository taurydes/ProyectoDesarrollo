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
public class ConsultarConsumidorPorCedulaQueryHandler : IRequestHandler<ConsultarConsumidorPorCedulaQuery, ConsumidorResponse>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<ConsultarConsumidorPorCedulaQueryHandler> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de la clase ConsultarConsumidorPorCedulaQueryHandler.
    /// </summary>
    /// <param name="dbContext">El contexto de la base de datos.</param>
    /// <param name="logger">El registrador.</param>
    public ConsultarConsumidorPorCedulaQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarConsumidorPorCedulaQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la consulta ConsultarUsuarioPorCorreoQuery y devuelve la información del usuario.
    /// </summary>
    /// <param name="request">La consulta ConsultarUsuarioPorCorreoQuery.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>El ConsumidorResponse que contiene la información del usuario.</returns>
    public async Task<ConsumidorResponse> Handle(ConsultarConsumidorPorCedulaQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                _logger.LogWarning("ConsultarUsuarioPorCorreoQueryHandler.Handle: Request nulo.");
                throw new RequestNullException("Request nulo");
            }
            else
            {
                return await HandleAsync(request.Cedula);
            }
        }
        catch (RequestNullException ex)
        {
            _logger.LogError(ex, "Request nulo en ConsultarConsumidoresQuery de ConsumidorController");
            throw;
        }
        catch (Exception)
        {
            _logger.LogWarning("ConsultarUsuarioPorCorreoQueryHandler.Handle: ArgumentNullException");
            throw;
        }
    }

    /// <summary>
    /// Recupera la información del usuario desde la base de datos en función del correo electrónico proporcionado.
    /// </summary>
    /// <param name="cedula">la cedula del usuario consumidor a consiultar.</param>
    /// <returns>El ConsumidorResponse que contiene la información del usuario.</returns>
    private async Task<ConsumidorResponse> HandleAsync(int cedula)
    {
        try
        {
            _logger.LogInformation("ConsultarUsuarioPorCorreoQueryHandler.HandleAsync");

            var usuario = await _dbContext.Consumidores.FirstOrDefaultAsync(u => u.Cedula == cedula);

            if (usuario is null)
            {
                throw new UsuarioNotFoundException("EL usuario no puede ser nulo");
            }

            var response = UsuarioMapper.MapEntityConsumidorAResponse(usuario);

            return response;

        }
        catch (UsuarioNotFoundException ex)
        {
            _logger.LogError(ex, "EL usuario no puede ser nulo");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ConsultarUsuarioPorCorreoQueryHandler.HandleAsync. {Mensaje}", ex.Message);
            throw;
        }
    }
}
