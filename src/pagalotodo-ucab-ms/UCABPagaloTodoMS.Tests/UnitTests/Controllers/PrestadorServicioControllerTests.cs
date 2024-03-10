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
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeaturePrestadorServicio;
using UCABPagaloTodoMS.Core.Database;

namespace UCABPagaloTodoMS.Tests.UnitTests.Controllers
{
    public class PrestadorServicioControllerTests
    {
        private readonly PrestadorServicioController _prestadorController;
        private readonly Mock<ILogger<PrestadorServicioController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<PrestadorServicioRequest>> _prestadorValidatorMock;
        private readonly Mock<IValidator<PrestadorServicioUpdateRequest>> _prestadorUpdateValidatorMock;
        private readonly IUCABPagaloTodoDbContext _dbcontext; // nueva


        public PrestadorServicioControllerTests()
        {
            _loggerMock = new Mock<ILogger<PrestadorServicioController>>();
            _mediatorMock = new Mock<IMediator>();
            _prestadorController = new PrestadorServicioController(_loggerMock.Object, _mediatorMock.Object, _dbcontext);
            _prestadorValidatorMock = new Mock<IValidator<PrestadorServicioRequest>>();
        }

        //PRUEBAS PARA EL METODO CREATE : 
        [Fact]
        public async Task Create_ValidPrestadorRequest_ReturnsOkResult()
        {
            // Arranque
            var prestadorRequest = new PrestadorServicioRequest
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = Guid.NewGuid(),
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789"
                //TODO: tenía un error xq le estaba pasando los datos que el validator no aceptaba 

            };

            var agregarPrestadorCommand = new AgregarPrestadorServicioCommand(prestadorRequest);

            _mediatorMock.Setup(m => m.Send(agregarPrestadorCommand, default)).ReturnsAsync(prestadorRequest.Id);

            // Act
            var result = await _prestadorController.Create(prestadorRequest);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Create_DeberiaRetornarBadRequestObjectResultCuandoOcurreUnError()
        {
            // Arrange
            var pretadorMock = new PrestadorServicioRequest
            {
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789"
            };
            var expectedException = new Exception($"Ocurrió un error en Create de PrestadorServicioController: {StatusCodes.Status400BadRequest}");

            var agregarConsumidorCommand = new AgregarPrestadorServicioCommand(pretadorMock);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AgregarPrestadorServicioCommand>(), default))
                .ThrowsAsync(expectedException);
            var loggerMock = _loggerMock;
            var prestadorController = _prestadorController;

            // Act
            var result = await prestadorController.Create(pretadorMock);

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
            var pretadorMock = new PrestadorServicioRequest
            {
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123",//clave demasiado corta
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789"
            };
            var expectedValidationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Clave",$"La clave debe tener al menos 8 caracteres.")
        };
            var validationResult = new ValidationResult(expectedValidationErrors);
            _prestadorValidatorMock
                 .Setup(v => v.Validate(It.IsAny<ValidationContext<PrestadorServicioRequest>>()))
                 .Returns(validationResult);
            var loggerMock = _loggerMock;
            var prestadorController = _prestadorController;

            // Act
            var result = await prestadorController.Create(pretadorMock);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
            var validationErrors = Assert.IsAssignableFrom<List<ValidationFailure>>(badRequestObjectResult.Value);
           
        }

        //PRUEBAS PARA EL METODO CONSULTAR  : 

        [Fact]
        public async Task ConsultarPrestadorQuery_RetornaListaDePrestadorResponse()
        {
            // Arrange
            var expectedResponse = new List<PrestadorServicioResponse>
            {
                new PrestadorServicioResponse
                {
                    Id = Guid.NewGuid(),
                    NombreUsuario = "usuario1",
                    Clave = "clave123456",
                    Correo = "correo1@test.com",
                    NombreEmpresa = "nombre1",
                    Rif = "V123456789",
                    ServiciosPrestados = new List<ServicioPrestado>(),

                },
                new PrestadorServicioResponse
                {
                    Id = Guid.NewGuid(),
                    NombreUsuario = "usuario1",
                    Clave = "clave123456",
                    Correo = "correo1@test.com",
                    NombreEmpresa = "nombre1",
                    Rif = "V123456789",
                    ServiciosPrestados = new List<ServicioPrestado>(),

                 },
            };
            
             _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarPrestadoresQuery>(), default))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _prestadorController.ConsultarPrestadoresQuery();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResponse = Assert.IsType<List<PrestadorServicioResponse>>(okObjectResult.Value);

        }

        [Fact]
        public async Task ConsultarPrestadorQuery_DeberiaRetornarBadRequestObjectResultCuandoOcurreExcepcion()
        {
            // Arrange
            var expectedErrorMessage = "Ocurrió un error en ConsultarPrestadoresQuery de PrestadorServicioController.";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarPrestadoresQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error de prueba"));
            var loggerMock = _loggerMock;
            var prestadorController = _prestadorController;

            // Act
            var result = await prestadorController.ConsultarPrestadoresQuery();

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
            var errorMessage = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        //PRUEBAS PARA EL METODO CONSULTAR POR ID  : 

        [Fact]
        public async Task ConsultarPrestadorPorIdQuery_RetornaActionResultConConsumidorResponse()
        {
            // Arrange
            var  id= Guid.NewGuid();
            var expectedResponse = new PrestadorServicioResponse
            {
                Id = id,
                NombreUsuario = "usuario1",
                Clave = "clave123456",
                Correo = "correo1@test.com",
                NombreEmpresa = "nombre1",
                Rif = "V123456789",
                ServiciosPrestados = new List<ServicioPrestado>(),
            };

            _mediatorMock
               .Setup(m => m.Send(It.IsAny<ConsultarPrestadoresPorIdQuery>(), default))
               .ReturnsAsync(expectedResponse);

            var loggerMock = _loggerMock;

            // Act
            var result = await _prestadorController.ConsultarPrestadoresPorIdQuery(id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResponse = Assert.IsType<PrestadorServicioResponse>(okObjectResult.Value);
            Assert.Equal(expectedResponse, actualResponse);
        }

        [Fact]
        public async Task ConsultarPrestadoresPorIdQuery_DeberiaRetornarBadRequestObjectResultCuandoOcurreExcepcion()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedErrorMessage = "Ocurrió un error en ConsultarPrestadoresPorIdQuery de PrestadorServicioController.";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarPrestadoresPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error de prueba"));
            var loggerMock = _loggerMock;
            var prestadorController = _prestadorController;

            // Act
            var result = await prestadorController.ConsultarPrestadoresPorIdQuery(id);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
            var errorMessage = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        //PRUEBAS PARA EL METODO ACTUALIZAR : 


        [Fact]
        public async Task Update_DeberiaRetornarOkConId()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var expectedRequest = new PrestadorServicioUpdateRequest
            {
                Id = expectedId,
                NombreUsuario = "Usuario1",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ActualizarPrestadorServicioCommand>(), default))
                .ReturnsAsync(expectedId);

            // Act
            var result = await _prestadorController.Update(expectedId, expectedRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedId, okResult.Value);
        }


        [Fact]
        public async Task Update_DeberiaRetornarBadRequestSiIdsNoCoinciden()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var request = new PrestadorServicioUpdateRequest
            {
                Id = Guid.NewGuid(),
                NombreUsuario = "Usuario1",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789"
            };

            // Act
            var result = await _prestadorController.Update(expectedId, request);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
        // Add more test methods here

        [Fact]
        public async Task Update_DeberiaRetornarBadRequestObjectResultCuandoOcurreExcepcion()
        {
            // Arrange
            var id = Guid.NewGuid();
            var consumidorUpdateRequest = new PrestadorServicioUpdateRequest { Id = id };
            var expectedErrorMessage = "Ocurrió un error en prestadorUpdaterequest de PrestadorServicioController.";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ActualizarPrestadorServicioCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error de prueba"));
            var loggerMock = _loggerMock;
            var prestadorController = _prestadorController;

            // Act
            var result = await prestadorController.Update(id, null);

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
    }
}

