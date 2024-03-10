using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;

public class ConsultarConfiguracionPagoPorIdQueryHandler : IRequestHandler<ConsultarConfiguracionPagoPorIdQuery, ConfiguracionPagoResponse>
{
    private readonly IUCABPagaloTodoDbContext _dbContext;
    private readonly ILogger<ConsultarConfiguracionPagoPorIdQueryHandler> _logger;

    public ConsultarConfiguracionPagoPorIdQueryHandler(IUCABPagaloTodoDbContext dbContext, ILogger<ConsultarConfiguracionPagoPorIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<ConfiguracionPagoResponse> Handle(ConsultarConfiguracionPagoPorIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                _logger.LogWarning("ConsultarUsuarioPorIdQueryHandler.Handle: Request nulo.");
                throw new ArgumentNullException(nameof(request));
            }
            else
            {
                return await HandleAsync(request.Id);
            }
        }
        catch (Exception)
        {
            _logger.LogWarning("ConsultarUsuarioPorIdQueryHandler.Handle: ArgumentNullException");
            throw;
        }
    }

    private async Task<ConfiguracionPagoResponse> HandleAsync(Guid id)
{
    try
    {
        _logger.LogInformation("ConsultarUsuarioPorIdQueryHandler.HandleAsync");

        var configuracionPago = await _dbContext.ConfiguracionPagos
            .Include(p => p.Campos)
            .FirstOrDefaultAsync(p => p.Id == id);

            if (configuracionPago is null)
            {
                throw new ArgumentNullException(nameof(configuracionPago));
            }

            var response = ConfiguracionPagoMapper.MapEntityAResponse(configuracionPago);

            return response;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error ConsultarUsuarioPorIdQueryHandler.HandleAsync. {Mensaje}", ex.Message);
        throw;
    }
    }
}