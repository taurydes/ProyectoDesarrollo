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
using UCABPagaloTodoMS.Core.Database;
using Moq.EntityFrameworkCore;

namespace UCABPagaloTodoMS.Tests.UnitTests.Controllers
{
    public class ConsumidorControllerTests
    {
        private readonly ConsumidorController _consumidorController;
        private readonly Mock<ILogger<ConsumidorController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<ConsumidorRequest>> _consumidorValidatorMock;
        private readonly Mock<IValidator<ConsumidorUpdateRequest>> _consumidorUpdateValidatorMock;
        private readonly IUCABPagaloTodoDbContext _dbcontext; // nueva



        public ConsumidorControllerTests()
        {
            _loggerMock = new Mock<ILogger<ConsumidorController>>();
            _mediatorMock = new Mock<IMediator>();
            _consumidorController = new ConsumidorController(_loggerMock.Object, _mediatorMock.Object, _dbcontext);
            _consumidorValidatorMock = new Mock<IValidator<ConsumidorRequest>>();

        }

        //PRUEBAS PARA EL METODO CREATE : 
        [Fact]
        public async Task Create_ValidConsumidorRequest_ReturnsOkResult()
        {
            // Arranque
            var consumidorRequest = new ConsumidorRequest
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = Guid.NewGuid(),
                NombreUsuario = "ACTUALIZANDO25",
                Clave = "123456789",
                Correo = "dani@gmail.com",
                Nombre = "ACTUALIZANDO",
                Apellido = "string",
                Telefono = 55555,
                Direccion = "string",
                EstatusCuenta = true
                       
                //TODO: tenía un error xq le estaba pasando los datos que el validator no aceptaba 

            };

            var agregarConsumidorCommand = new AgregarConsumidorCommand(consumidorRequest);

            _mediatorMock.Setup(m => m.Send(agregarConsumidorCommand, default)).ReturnsAsync(consumidorRequest.Id);

            // Act
            var result = await _consumidorController.Create(consumidorRequest);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Create_DeberiaRetornarBadRequestObjectResultCuandoOcurreUnError()
        {
            // Arrange
            var consumidorMock = new ConsumidorRequest
            {
                NombreUsuario = "usuario1",
                Clave = "clave123456",
                Correo = "correo1@test.com",
                Nombre = "nombre1",
                Apellido = "apellido1",
                Telefono = 1234567,
                Direccion = "dirección1",
                EstatusCuenta = true,
            };
            var expectedException = new Exception($"Ocurrió un error en Create de ConsumidorController: {StatusCodes.Status400BadRequest}");

            var agregarConsumidorCommand = new AgregarConsumidorCommand(consumidorMock);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AgregarConsumidorCommand>(), default))
                .ThrowsAsync(expectedException);     
            // Act
            var result = await _consumidorController.Create(consumidorMock);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
            var errorMessage = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.Equal(expectedException.Message, errorMessage);
        }

        [Fact]
        public async Task Create_DeberiaRetornarBadRequestObjectResultCuandoValidationResultNoEsValido()
        {
            // Arrange
            var consumidorMock = new ConsumidorRequest
            {
                NombreUsuario = "usuario1",
                Clave = "abc", // Clave demasiado corta
                Correo = "correo1@test.com",
                Nombre = "nombre1",
                Apellido = "apellido1",
                Telefono = 123456,
                Direccion = "dirección1",
                EstatusCuenta = true,
            };
            var expectedValidationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Clave",$"La clave debe tener al menos 8 caracteres.")
        };
            var validationResult = new ValidationResult(expectedValidationErrors);
           _consumidorValidatorMock           
                .Setup(v => v.Validate(It.IsAny<ValidationContext<ConsumidorRequest>>()))
                .Returns(validationResult);
            
            // Act
            var result = await _consumidorController.Create(consumidorMock);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
            var validationErrors = Assert.IsAssignableFrom<List<ValidationFailure>>(badRequestObjectResult.Value);
           
        }

        //PRUEBAS PARA EL METODO CONSULTAR CONSUMIDORES : 

        [Fact]
        public async Task ConsultarConsumidorQuery_RetornaListaDeConsumidorResponse()
        {
            // Arrange
            var expectedResponse = new List<ConsumidorResponse>
            {
                new ConsumidorResponse
                {
                    Id = Guid.NewGuid(),
                    NombreUsuario = "usuario1",
                    Clave = "clave123456",
                    Correo = "correo1@test.com",
                    Nombre = "nombre1",
                    Apellido = "apellido1",
                    Telefono = 1234567,
                    Direccion = "dirección1",
                    EstatusCuenta = true,
                    PagosRealizados = new List<Pago>(),
                    PagosPendientes = new List<ArchivoDeuda>(),
                },
                new ConsumidorResponse
                {
                    Id = Guid.NewGuid(),
                    NombreUsuario = "usuario2",
                    Clave = "clave123456",
                    Correo = "correo2@test.com",
                    Nombre = "nombre2",
                    Apellido = "apellido2",
                    Telefono = 9876543,
                    Direccion = "dirección2",
                    EstatusCuenta = false,
                    PagosRealizados = new List<Pago>(),
                    PagosPendientes = new List<ArchivoDeuda>(),
                }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarConsumidoresQuery>(), default))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _consumidorController.ConsultarConsumidorQuery();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResponse = Assert.IsType<List<ConsumidorResponse>>(okObjectResult.Value);
            
            
        }

        [Fact]
        public async Task ConsultarConsumidorQuery_DeberiaRetornarBadRequestObjectResultCuandoOcurreExcepcion()
        {
            // Arrange
            var expectedErrorMessage = "Ocurrió un error en ConsultarConsumidorQuery de ConsumidorController.";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarConsumidoresQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error de prueba"));
                      
            // Act
            var result = await _consumidorController.ConsultarConsumidorQuery();

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
            var errorMessage = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        //PRUEBAS PARA EL METODO CONSULTAR CONSUMIDORES POR ID  : 

        [Fact]
        public async Task ConsultarConsumidorPorIdQuery_RetornaActionResultConConsumidorResponse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResponse = new ConsumidorResponse
            {
                Id = id,
                NombreUsuario = "usuario1",
                Clave = "clave123456",
                Correo = "correo1@test.com",
                Nombre = "nombre1",
                Apellido = "apellido1",
                Telefono = 1234567,
                Direccion = "dirección1",
                EstatusCuenta = true,
                PagosRealizados = new List<Pago>(),
                PagosPendientes = new List<ArchivoDeuda>(),
            };

            _mediatorMock
               .Setup(m => m.Send(It.IsAny<ConsultarConsumidoresPorIdQuery>(), default))
               .ReturnsAsync(expectedResponse);

           
            // Act
            var result = await _consumidorController.ConsultarConsumidoresPorIdQuery(id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResponse = Assert.IsType<ConsumidorResponse>(okObjectResult.Value);
            Assert.Equal(expectedResponse, actualResponse);
        }

        [Fact]
        public async Task ConsultarPrestadoresPorIdQuery_DeberiaRetornarBadRequestObjectResultCuandoOcurreExcepcion()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedErrorMessage = "Ocurrió un error en GetById de ConsumidorController : Error de prueba";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarConsumidoresPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error de prueba"));
           
           

            // Act
            var result = await _consumidorController.ConsultarConsumidoresPorIdQuery(id);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
            var errorMessage = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        //PRUEBAS PARA EL METODO APTUALIZAR CONSUMIDORES : 


        [Fact]
        public async Task Update_DeberiaRetornarOkConId()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var expectedRequest = new ConsumidorUpdateRequest
            {
                NombreUsuario = "usuario1",
                Clave = "123456789",
                Correo = "correo1@test.com",
                Id = expectedId,
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = 123456,
                Direccion = "Calle 123",
                EstatusCuenta = true,
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ActualizarConsumidorCommand>(), default))
                .ReturnsAsync(expectedId);

            // Act
            var result = await _consumidorController.Update(expectedId, expectedRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedId, okResult.Value);
        }


        [Fact]
        public async Task Update_DeberiaRetornarBadRequestSiIdsNoCoinciden()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var request = new ConsumidorUpdateRequest
            {
                NombreUsuario = "usuario1",
                Clave = "123456789",
                Correo = "correo1@test.com",
                Id = Guid.NewGuid(),
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = 123456,
                Direccion = "Calle 123",
                EstatusCuenta = true,
            };

            // Act
            var result = await _consumidorController.Update(expectedId, request);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
        // Add more test methods here

        [Fact]
        public async Task Update_DeberiaRetornarBadRequestObjectResultCuandoOcurreExcepcion()
        {
            // Arrange
            var id = Guid.NewGuid();
            var consumidorUpdateRequest = new ConsumidorUpdateRequest { Id = id };
           
            var expectedErrorMessage = "Ocurrió un error en consumidorupdaterequest de ConsumidorController.";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ActualizarConsumidorCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error de prueba"));
         
            // Act
            var result = await _consumidorController.Update(id, null);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
            var errorMessage = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.Equal(expectedErrorMessage, errorMessage);
        }




        // Add more test methods here

        //[Fact]
        //public async Task Update_DeberiaRetornarBadRequestObjectResultCuandoValidationResultNoEsValido()
        //{
        //    // Arrange
        //    var id = Guid.NewGuid();
        //    var consumidorUpdateRequest = new ConsumidorUpdateRequest
        //    {
        //        Id = id,
        //        NombreUsuario = "usuario1",
        //        Clave = "abc", // Clave demasiado corta
        //        Correo = "correo1@test.com",
        //        Nombre = "nombre1",
        //        Apellido = "apellido1",
        //        Telefono = 123456,
        //        Direccion = "dirección1",
        //        EstatusCuenta = true,
        //    };
        //    var expectedValidationErrors = new List<ValidationFailure>
        //    {
        //        new ValidationFailure("Clave",$"La clave debe tener al menos 8 caracteres.")
        //    };
        //    var validationResult = new ValidationResult(expectedValidationErrors);
        //    _consumidorUpdateValidatorMock
        //        .Setup(v => v.Validate(It.IsAny<ValidationContext<ConsumidorUpdateRequest>>()))
        //        .Returns(validationResult);
        //    var loggerMock = _loggerMock;
        //    var consumidorController = _consumidorController;

        //    // Act
        //    var result = await consumidorController.Update(id, consumidorUpdateRequest);

        //    // Assert
        //    var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
        //    var validationErrors = Assert.IsAssignableFrom<List<ValidationFailure>>(badRequestObjectResult.Value);
        //    Assert.Equal(expectedValidationErrors, validationErrors);
        //}

        [Fact]
        public async Task CambiaEstatus_DeberiaRetornarOk()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var expectedConsumidorResponse = new ConsumidorResponse
            {
                Id = expectedId,
                NombreUsuario = "usuario1",
                Clave = "123456789",
                Correo = "correo1@test.com",
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = 123456,
                Direccion = "Calle 123",
                EstatusCuenta = true,
            };

            var _mediatorMock_2 = _mediatorMock;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarConsumidoresPorIdQuery>(), default))
                .ReturnsAsync(expectedConsumidorResponse);

            _mediatorMock_2
                .Setup(m => m.Send(It.IsAny<CambiarEstatusCommand>(), default))
                .ReturnsAsync(expectedId);

            // Act
            var result = await _consumidorController.CambiaEstatus(expectedId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task CambiaEstatus_DeberiaRetornarBadRequestSiIdNoExiste()
        {
            // Arrange
            var idInexistente = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarConsumidoresPorIdQuery>(), default))
                .ReturnsAsync((ConsumidorResponse)null);

            // Act
            var result = await _consumidorController.CambiaEstatus(idInexistente);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Ocurrió no se encuentra el Usuario.", badRequestResult.Value);
        }

        [Fact]
        public async Task CambiaEstatus_DeberiaRetornarBadRequestSiOcurreExcepcion()
        {
            // Arrange
            var expectedId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarConsumidoresPorIdQuery>(), default))
                .ThrowsAsync(new Exception("Error de prueba"));

            // Act
            var result = await _consumidorController.CambiaEstatus(expectedId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Ocurrió un error en CambiaEstatus de ConsumidorController.", badRequestResult.Value);
        }
    }

}


