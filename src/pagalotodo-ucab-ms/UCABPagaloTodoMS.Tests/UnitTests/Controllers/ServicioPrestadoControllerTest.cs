using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeaturePrestadorServicio;
using UCABPagaloTodoMS.Application.Commands.FeatureServicioPrestado;
using UCABPagaloTodoMS.Application.Commands.FeatureServiciosPrestados;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Controllers;
using Xunit;

namespace UCABPagaloTodoMS.Tests.UnitTests.Controllers
{
    public class ServicioPrestadoControllerTests
    {
        private readonly ServicioPrestadoController _servicioController;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<ServicioPrestadoController>> _loggerMock;
        private readonly Mock<IValidator<CrearServicioPrestadoRequest>> _validatorMock;

        public ServicioPrestadoControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<ServicioPrestadoController>>();
            _validatorMock = new Mock<IValidator<CrearServicioPrestadoRequest>>();
            _servicioController = new ServicioPrestadoController(_loggerMock.Object, _mediatorMock.Object);
        }

        // Resto de las pruebas unitarias...

        // PRUEBAS PARA EL MÉTODO Create

        [Fact]
        public async Task Create_ReturnsOkResult_WhenServiceCreationSucceeds()
        {
            // Arrange
            var request = new CrearServicioPrestadoRequest
            {
                Id = Guid.NewGuid(),
                Nombre = "Servicio de prueba",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,
                PrestadorServicioId = Guid.NewGuid(),
            };
            var expectedResponse = Guid.NewGuid();

            _validatorMock.Setup(v => v.Validate(It.IsAny<ValidationContext<CrearServicioPrestadoRequest>>()))
                .Returns(new ValidationResult());

            _mediatorMock.Setup(m => m.Send(It.IsAny<CrearServicioPrestadoCommand>(), default))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _servicioController.Create(request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Guid>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(expectedResponse, okObjectResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsBadRequestResult_WhenValidationFails()
        {
            // Arrange
            var invalidServicio = new CrearServicioPrestadoRequest
            {
                PrestadorServicioId = Guid.Empty, // El ID del prestador de servicio no puede estar vacío.
                Nombre = "prueba", // Valor inválido para el nombre del servicio
                Descripcion = "prueba", // Valor inválido para la descripción del servicio
                Costo = 0, // Valor inválido para el costo del servicio
                EstadoServicio = "activo",
                TipoPago = true
            };

            var expectedValidationErrors = new List<ValidationFailure>
            {
            new ValidationFailure("PrestadorServicioId",$"El ID del prestador de servicio no puede estar vacío.")
            };

            var validationResult = new ValidationResult(expectedValidationErrors);
            _validatorMock
                 .Setup(v => v.Validate(It.IsAny<ValidationContext<ServicioPrestadoRequest>>()))
                 .Returns(validationResult);
            // Act
            var result = await _servicioController.Create(invalidServicio);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
            var validationErrors = Assert.IsAssignableFrom<List<ValidationFailure>>(badRequestObjectResult.Value);

        }

        [Fact]
        public async Task ConsultarServiciosQuery_RetornaListaDeServicioPrestadoResponse()
        {
            // Arrange
            var expectedResponse = new List<ServicioPrestadoResponse>
            {
                new ServicioPrestadoResponse
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Servicio 1",
                    Descripcion = "Descripción del servicio 1",
                    Costo = 10,
                    EstadoServicio = "activo",
                    TipoPago = true,
                },
                new ServicioPrestadoResponse
                {
                    Id = Guid.NewGuid(),
                    Nombre = "Servicio 2",
                    Descripcion = "Descripción del servicio 2",
                    Costo = 15,
                    EstadoServicio = "inactivo",
                    TipoPago = false
                }
            };

        
            _mediatorMock.Setup(m => m.Send(It.IsAny<ConsultarServicioPrestadoQuery>(), default))
                .ReturnsAsync(expectedResponse);

              // Act

            var result = await _servicioController.ConsultarServiciosQuery();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResponse = Assert.IsType<List<ServicioPrestadoResponse>>(okObjectResult.Value);

            Assert.Equal(expectedResponse.Count, actualResponse.Count);
            Assert.Equal(expectedResponse[0].Id, actualResponse[0].Id);
            Assert.Equal(expectedResponse[0].Nombre, actualResponse[0].Nombre);
            Assert.Equal(expectedResponse[0].Descripcion, actualResponse[0].Descripcion);
            Assert.Equal(expectedResponse[0].Costo, actualResponse[0].Costo);
            Assert.Equal(expectedResponse[0].EstadoServicio, actualResponse[0].EstadoServicio);
            Assert.Equal(expectedResponse[0].TipoPago, actualResponse[0].TipoPago);
            Assert.Equal(expectedResponse[1].Id, actualResponse[1].Id);
            Assert.Equal(expectedResponse[1].Nombre, actualResponse[1].Nombre);
            Assert.Equal(expectedResponse[1].Descripcion, actualResponse[1].Descripcion);
            Assert.Equal(expectedResponse[1].Costo, actualResponse[1].Costo);
            Assert.Equal(expectedResponse[1].EstadoServicio, actualResponse[1].EstadoServicio);
            Assert.Equal(expectedResponse[1].TipoPago, actualResponse[1].TipoPago);
        }

        //PRUEBAS PARA EL METODO CONSULTAR POR ID  : 
        [Fact]
        public async Task ConsultarServicioPrestadoPorIdQuery_RetornaServicioPrestadoResponse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResponse = new ServicioPrestadoResponse
            {
                Id = id,
                Nombre = "Servicio 1",
                Descripcion = "Descripción del servicio 1",
                Costo = 10.99F,
                EstadoServicio = "activo",
                TipoPago = true
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarServicioPrestadoPorIdQuery>(), default))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _servicioController.ConsultarServicioPrestadoPorIdQuery(id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResponse = Assert.IsType<ServicioPrestadoResponse>(okObjectResult.Value);

            Assert.Equal(expectedResponse.Id, actualResponse.Id);
            Assert.Equal(expectedResponse.Nombre, actualResponse.Nombre);
            Assert.Equal(expectedResponse.Descripcion, actualResponse.Descripcion);
            Assert.Equal(expectedResponse.Costo, actualResponse.Costo);
            Assert.Equal(expectedResponse.EstadoServicio, actualResponse.EstadoServicio);
            Assert.Equal(expectedResponse.TipoPago, actualResponse.TipoPago);
        }

        [Fact]
        public async Task ConsultarServicioPrestadoPorIdQuery_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var id = Guid.NewGuid();

            _loggerMock
                .Setup(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Verifiable();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarServicioPrestadoPorIdQuery>(), default))
                .ThrowsAsync(new Exception());

            var controller = new ServicioPrestadoController(_loggerMock.Object, _mediatorMock.Object);

            // Act
            var result = await controller.ConsultarServicioPrestadoPorIdQuery(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Ocurrió un error en ConsultarServicioPrestadoPorIdQuery de ServicioPrestadoController.", badRequestResult.Value);

            _loggerMock.Verify();
        }

        [Fact]
        public async Task Update_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var servicio_new = new ServicioPrestadoUpdateRequest
            {
                Id = id,
                Nombre = "Nuevo nombre",
                Descripcion = "Nueva descripción",
                Costo = 10.99f,
                EstadoServicio = "activo",
                TipoPago = true
            };

            // Configuración del comportamiento de _mediatorMock
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ActualizarServicioPrestadoCommand>(), default))
                .ReturnsAsync(id);

            // Act
            var result = await _servicioController.Update(id, servicio_new);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var id = Guid.NewGuid();
            var servicio_new = new ServicioPrestadoUpdateRequest
            {
                Id = Guid.NewGuid(), // ID diferente al esperado
                Nombre = "Nuevo Nombre",
                Descripcion = "Nueva Descripción",
                Costo = 10.99f,
                EstadoServicio = "activo",
                TipoPago = true
            };


            // Act
            var result = await _servicioController.Update(id, servicio_new);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }
        [Fact]
        public async Task Delete_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await _servicioController.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("servicio eliminado", okResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequestResult_OnException()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Configuración del comportamiento de _mediatorMock
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<EliminarServicioPrestadoCommand>(), default))
                .Throws(new Exception());

            // Act
            var result = await _servicioController.Delete(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Ocurrió un error en Delete de ServicioPrestadoController.", badRequestResult.Value);
        }

    }

    
}
