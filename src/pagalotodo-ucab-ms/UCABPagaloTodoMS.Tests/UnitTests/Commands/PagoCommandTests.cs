using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCABPagaloTodoMS.Application.Commands.FeatureConsumidor;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Controllers;
using Xunit;
using MediatR;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using Microsoft.Azure.Amqp.Transaction;
using UCABPagaloTodoMS.Core.Entities;
using Microsoft.AspNetCore.Routing;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentAssertions;
using UCABPagaloTodoMS.Application.Validators;
using FluentValidation.Results;
using ValidationResult = FluentValidation.Results.ValidationResult;
using Microsoft.AspNetCore.Http;
using UCABPagaloTodoMS.Application.Commands.FeaturePago;
using UCABPagaloTodoMS.Application.Handlers.Commands.FeaturePago;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Mappers;

namespace UCABPagaloTodoMS.Tests.UnitTests.Commands
{
    public class PagoCommandTests
    {
        private readonly PagoController _pagoController;
        private readonly Mock<ILogger<PagoController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<ConsumidorUpdateRequest>> _consumidorUpdateValidatorMock;
        private readonly Mock<IValidator<ConsumidorRequest>> _pagoValidatorMock;
        private readonly Mock<IUCABPagaloTodoDbContext> _dbcontextMock;
        private readonly Mock<IDbContextTransactionProxy> _mockProxy;
        private readonly CrearPagoCommandHandler _handler;
       


        public PagoCommandTests()
        {
            _loggerMock = new Mock<ILogger<PagoController>>();
            _mediatorMock = new Mock<IMediator>();
            _pagoController = new PagoController(_loggerMock.Object, _mediatorMock.Object);
            _pagoValidatorMock = new Mock<IValidator<ConsumidorRequest>>();
           
            _dbcontextMock = new Mock<IUCABPagaloTodoDbContext>();
            _mockProxy = new Mock<IDbContextTransactionProxy>();
            _handler = new CrearPagoCommandHandler(_dbcontextMock.Object, Mock.Of<ILogger<CrearPagoCommandHandler>>());
        
        }

        [Fact]
        public async Task AgregarPagoCommandHandler_Handle_ValidRequestd()
        {
            // Arrange
            var request = new CrearPagoCommand(new CrearPagoRequest()
            {
                Referencia = "Compra1",
                ConsumidorId = Guid.NewGuid(),
                ServicioPrestadoId = Guid.NewGuid(),
            });

            var servicio = PagoMapper.MapRequestPagoEntity(request.Pago);
            _dbcontextMock.Setup(s => s.Pagos.Add(servicio));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);
            var result = await _handler.Handle(request, default);
            // Assert
            Assert.IsType<Guid>(result);
        }

        [Fact]
        public async Task AgregarPagoCommandHandler_Handle_ExceptionThrown()
        {
            // Arrange
            var request = new CrearPagoCommand(new CrearPagoRequest()
            {
              
                Referencia = "Compra1",
                ConsumidorId = Guid.NewGuid(),
                ServicioPrestadoId = Guid.NewGuid(),
            });

            var servicio = PagoMapper.MapRequestPagoEntity(request.Pago);
            _dbcontextMock.Setup(s => s.Pagos.Add(servicio));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ThrowsAsync(new Exception("Error al guardar el Pago"));

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(request, default));
        }

        [Fact]
        public async Task AgregarPagoCommandHandler_Handle_ExceptionThrownInHandle()
        {
            // Arrange
            var request = new CrearPagoCommand(new CrearPagoRequest()
            {
              
                Referencia = "Compra1",
                ConsumidorId = Guid.NewGuid(),
                ServicioPrestadoId = Guid.NewGuid(),
            });

            _dbcontextMock.Setup(s => s.Pagos.Add(It.IsAny<Pago>()));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(request, default));
        }
    }

}


