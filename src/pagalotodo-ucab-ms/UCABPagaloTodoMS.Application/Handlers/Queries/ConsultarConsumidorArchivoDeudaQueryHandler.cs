using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;

/// <summary>
/// Maneja la consulta ConsultarConsumidorArchivoDeudaQuery al recuperar información del Archivo Deuda desde la base de datos.
/// </summary>
public class ConsultarConsumidorArchivoDeudaQueryHandler : IRequestHandler<ConsultarConsumidorArchivoDeudaQuery, bool>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<ConsultarConsumidorArchivoDeudaQueryHandler> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de la clase ConsultarConsumidorArchivoDeudaQueryHandler.
    /// </summary>
    /// <param name="dbContext">El contexto de la base de datos.</param>
    /// <param name="logger">El registrador.</param>
    public ConsultarConsumidorArchivoDeudaQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarConsumidorArchivoDeudaQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la consulta ConsultarConsumidorArchivoDeudaQuery y devuelve la información del archivo de deudas.
    /// </summary>
    /// <param name="request">La consulta ConsultarConsumidorArchivoDeudaQuery.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>El valor booleano que retorne la consulta.</returns>
    public async Task<bool> Handle(ConsultarConsumidorArchivoDeudaQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                _logger.LogWarning("ConsultarConsumidorArchivoDeudaQueryHandler.Handle: Request nulo.");
                throw new ArgumentNullException(nameof(request));
            }
            else
            {
                return await HandleAsync(request.ConsultaArchivo);
            }
        }
        catch (Exception)
        {
            _logger.LogWarning("ConsultarConsumidorArchivoDeudaQueryHandler.Handle: ArgumentNullException");
            throw;
        }
    }

    /// <summary>
    /// Busca si tanto el consumidor como el servicio se encuentran asociados en la tabla archivoDeuda en la base de datos en función de los datos proporcionados.
    /// </summary>
    /// <param name="ConsultaArchivo">los datos necesarios para la consulta servicioPrestadoId y ConsumidorId.</param>
    /// <returns>El valor booleano si se encuentra el registro o no.</returns>
    private async Task<bool> HandleAsync(ConsumidorArchivoDeudaRequest ConsultaArchivo)
    {
        try
        {
            _logger.LogInformation("ConsultarConsumidorArchivoDeudaQueryHandler.HandleAsync");

            var consulta = await _dbContext.ArchivoDeudas.FirstOrDefaultAsync(u => u.ServicioPrestadoId == ConsultaArchivo.ServicioPrestadoId && u.ConsumidorId == ConsultaArchivo.ConsumidorId);

            if (consulta is null) return false;

            return true;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ConsultarConsumidorArchivoDeudaQueryHandler.HandleAsync. {Mensaje}", ex.Message);
            throw;
        }
    }
}
