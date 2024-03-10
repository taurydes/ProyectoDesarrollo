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

namespace UCABPagaloTodoMS.Tests.UnitTests.Controllers
{
    public class PagoControllerTests
    {
        private readonly PagoController _pagoController;
        private readonly Mock<ILogger<PagoController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<ConsumidorUpdateRequest>> _consumidorUpdateValidatorMock;
        private readonly Mock<IValidator<ConsumidorRequest>> _pagoValidatorMock;



        public PagoControllerTests()
        {
            _loggerMock = new Mock<ILogger<PagoController>>();
            _mediatorMock = new Mock<IMediator>();
            _pagoController = new PagoController(_loggerMock.Object, _mediatorMock.Object);
            _pagoValidatorMock = new Mock<IValidator<ConsumidorRequest>>();

        }

        //PRUEBAS PARA EL METODO CREATE : 
        [Fact]
        public async Task Create_ValidConsumidorRequest_ReturnsOkResult()
        {
            var id = Guid.NewGuid();
            // Arranque
            var pagoMock = new CrearPagoRequest
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Referencia = "Compra1",
                ConsumidorId = Guid.NewGuid(),
                ServicioPrestadoId = Guid.NewGuid(),
   
            };

            var agregarPagoCommand = new CrearPagoCommand(pagoMock);

            _mediatorMock.Setup(m => m.Send(agregarPagoCommand, default)).ReturnsAsync(id);

            // Act
            var result = await _pagoController.Create(pagoMock);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Create_Exception_ReturnsBadRequest()
        {
            // Arrange
            var pagoMock = new CrearPagoRequest
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
               
             
                Referencia = "Compra1",
                ConsumidorId = Guid.NewGuid(),
                ServicioPrestadoId = Guid.NewGuid(),
            };
            var agregarPagoCommand = new CrearPagoCommand(pagoMock);

            _mediatorMock.Setup(m => m.Send(agregarPagoCommand, default)).ThrowsAsync(new Exception("Error al crear el pago"));

            // Act
            var result = await _pagoController.Create(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Ocurrió un error en Create de PagoController: 400", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_DeberiaRetornarBadRequestObjectResultCuandoValidationResultNoEsValido()
        {
            // Arrange
            var pagoMock = new CrearPagoRequest
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
               
                Referencia = "Compra1",
                ConsumidorId = Guid.NewGuid(),
                ServicioPrestadoId = Guid.NewGuid(),
            };
            
            var expectedValidationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Monto",$"El monto debe ser mayor a cero.")
        };
            var validationResult = new ValidationResult(expectedValidationErrors);
            _pagoValidatorMock
                 .Setup(v => v.Validate(It.IsAny<ValidationContext<CrearPagoRequest>>()))
                 .Returns(validationResult);

            // Act
            var result = await _pagoController.Create(pagoMock);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
            var validationErrors = Assert.IsAssignableFrom<List<ValidationFailure>>(badRequestObjectResult.Value);

        }

    }

}


