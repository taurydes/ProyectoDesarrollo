using Azure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Exceptions;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Application.Handlers.Queries;
public class ConsultarPagosAprobadoQueryHandler : IRequestHandler<ConsultarPagosAprobadoQuery, ServicioPrestadoPagosRangoResponse>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<ConsultarPagosAprobadoQueryHandler> _logger;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ConsultarPagosAprobadoQueryHandler"/>.
    /// </summary>
    /// <param name="dbContext">El contexto de la base de datos.</param>
    /// <param name="logger">El registrador.</param>
    public ConsultarPagosAprobadoQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarPagosAprobadoQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Maneja la consulta de ConsultarPagosServicioPrestadoPorRangoDetalleQuery y recupera un ServicioPrestadoPagosRangoResponse.
    /// </summary>
    /// <param name="request">La consulta de ConsultarPagosServicioPrestadoPorRangoDetalleQuery.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Una tarea que representa la operación asíncrona con el ServicioPrestadoPagosRangoResponse.</returns>
    public async Task<ServicioPrestadoPagosRangoResponse> Handle(ConsultarPagosAprobadoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                _logger.LogWarning("ConsultarUsuarioPorIdQueryHandler.Handle: Request nulo.");
                throw new RequestNullException("El Request es nulo");
            }
            else
            {
                return await HandleAsync(request);
            }
        }
            catch (RequestNullException ex)
        {
            _logger.LogWarning("El Request es nulo");
            throw;
        }
        catch (Exception)
        {
            _logger.LogWarning("ConsultarUsuarioPorIdQueryHandler.Handle: ArgumentNullException");
            throw;
        }
    }

    /// <summary>
    /// metodo asincrono que realiza la busqueda en Bd y lo forma con el response requerido
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ServicioPrestadoNotFoundException"></exception>
    private async Task<ServicioPrestadoPagosRangoResponse> HandleAsync(ConsultarPagosAprobadoQuery request)
    {
        try
        {
            var fechaInicio = request.DatosServicio.FechaInicio;
            // con el AddDay le agregamos el día actual de la fecha fin y le restamos un segundo para que tome encuanta todos los pagos q eesten en la fecha fín, anteriormente no los tomaba
            var fechaFin = request.DatosServicio.FechaFin.AddDays(1);
            _logger.LogInformation("ConsultarUsuarioPorIdQueryHandler.HandleAsync");

            var servicio = await _dbContext.ServiciosPrestados
                .Include(p => p.PagosPorServicioRealizados.Where(p => p.CreatedAt >= fechaInicio && p.CreatedAt <= fechaFin && p.EstadoPago == "aprobado"))
                .ThenInclude(p => p.CamposPago.Where(c => request.DatosServicio.CamposRequeridos.Contains(c.NombreCampo)))
                .FirstOrDefaultAsync(p => p.Id == request.DatosServicio.ServicioPrestadoId);

            var prestadorServicio = await _dbContext.PrestadorServicios.FirstOrDefaultAsync(p => p.Id == servicio.PrestadorServicioId);

            if (servicio is null)
            {
                throw new ServicioPrestadoNotFoundException($"No se encontró el servicio prestado con el ID: {request.DatosServicio.ServicioPrestadoId}");
            }

            var response = ServicioPrestadoMapper.MapEntityServicioRangoResponse(servicio, prestadorServicio.NombreEmpresa ?? "No Disponible", prestadorServicio.Correo ?? "dtoro1996@gmail.com");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ConsultarUsuarioPorIdQueryHandler.HandleAsync. {Mensaje}", ex.Message);
            throw;
        }
    }
}