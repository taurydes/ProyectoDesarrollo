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
using UCABPagaloTodoMS.Application.Handlers.Commands.FeatureAdministrador;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Infrastructure.Database;
using UCABPagaloTodoMS.Application.Mappers;

namespace UCABPagaloTodoMS.Tests.UnitTests.Controllers
{
    public class AdministradorControllerTests
    {
        private readonly AdministradorController _administradorController;
        private readonly Mock<ILogger<AdministradorController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<AdministradorRequest>> _administradorValidatorMock;
        private readonly Mock<IValidator<AdministradorUpdateRequest>> _administradorUpdateValidatorMock;
        private readonly Mock<IUCABPagaloTodoDbContext> _dbcontextMock;
        private readonly IUCABPagaloTodoDbContext _dbcontext; // nueva


        public AdministradorControllerTests()
        {
            _loggerMock = new Mock<ILogger<AdministradorController>>();
            _mediatorMock = new Mock<IMediator>();
            _administradorController = new AdministradorController(_loggerMock.Object, _mediatorMock.Object, _dbcontext);
            _administradorValidatorMock = new Mock<IValidator<AdministradorRequest>>();
            _dbcontextMock = new Mock<IUCABPagaloTodoDbContext>();
        }

        //PRUEBAS PARA EL METODO CREATE : 
        [Fact]
        public async Task Create_ValidPrestadorRequest_ReturnsOkResult()
        {
            // Arranque
            var admistradorRequest = new AdministradorRequest
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = Guid.NewGuid(),
                NombreUsuario = "Administrador5",
                Clave = "123456789",
                Correo = "test5@gmail.com",
                NombreAdministrador = "AdminPrueba",
              
                //TODO: tenía un error xq le estaba pasando los datos que el validator no aceptaba 

            };

            var agregarAdministradorCommand = new AgregarAdministradorCommand(admistradorRequest);

            _mediatorMock.Setup(m => m.Send(agregarAdministradorCommand, default)).ReturnsAsync(admistradorRequest.Id);

            // Act
            var result = await _administradorController.Create(admistradorRequest);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }



        [Fact]
        public async Task Create_DeberiaRetornarBadRequestObjectResultCuandoOcurreUnError()
        {
            // Arrange
            var administradorMock = new AdministradorRequest
            {
                NombreUsuario = "Administrador",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreAdministrador = "AdminPrueba",
            };
            var expectedException = new Exception($"Ocurrió un error en Create de AdministradorController: {StatusCodes.Status400BadRequest}");

            var agregarAdministradorCommand = new AgregarAdministradorCommand(administradorMock);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AgregarAdministradorCommand>(), default))
                .ThrowsAsync(expectedException);
            var loggerMock = _loggerMock;
            var administradorController = _administradorController;

            // Act
            var result = await administradorController.Create(administradorMock);

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
            var administradorMock = new AdministradorRequest
            {
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123",//clave demasiado corta
                Correo = "daniel@gmail.com",
                NombreAdministrador = "AdminPrueba",
            };
            var expectedValidationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Clave",$"La clave debe tener al menos 8 caracteres.")
        };
            var validationResult = new ValidationResult(expectedValidationErrors);
             _administradorValidatorMock
                 .Setup(v => v.Validate(It.IsAny<ValidationContext<AdministradorRequest>>()))
                 .Returns(validationResult);
            var loggerMock = _loggerMock;
            var administradorController = _administradorController;

            // Act
            var result = await administradorController.Create(administradorMock);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
            var validationErrors = Assert.IsAssignableFrom<List<ValidationFailure>>(badRequestObjectResult.Value);
           
        }

        //PRUEBAS PARA EL METODO CONSULTAR  : 

        [Fact]
        public async Task ConsultarAdministradorQuery_RetornaListaDePrestadorResponse()
        {
            // Arrange
            var expectedResponse = new List<AdministradorResponse>
            {
                new AdministradorResponse
                {
                    Id = Guid.NewGuid(),
                    NombreUsuario = "usuario1",
                    Clave = "clave123456",
                    Correo = "correo1@test.com",
                    NombreAdministrador = "AdminPrueba",

                },
                new AdministradorResponse
                {
                    Id = Guid.NewGuid(),
                    NombreUsuario = "usuario1",
                    Clave = "clave123456",
                    Correo = "correo1@test.com",
                    NombreAdministrador = "AdminPrueba",

                 },
            };
            
             _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarAdministradoresQuery>(), default))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _administradorController.ConsultarAdministradoresQuery();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResponse = Assert.IsType<List<AdministradorResponse>>(okObjectResult.Value);

        }

        [Fact]
        public async Task ConsultarAdministradorQuery_DeberiaRetornarBadRequestObjectResultCuandoOcurreExcepcion()
        {
            // Arrange
            var expectedErrorMessage = "Ocurrió un error en ConsultarAdministradorQuery de AdministradorController.";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarAdministradoresQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error de prueba"));
            var loggerMock = _loggerMock;
            var administradorController = _administradorController;

            // Act
            var result = await administradorController.ConsultarAdministradoresQuery();

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
            var expectedResponse = new AdministradorResponse
            {
                Id = id,
                NombreUsuario = "usuario1",
                Clave = "clave123456",
                Correo = "correo1@test.com",
                NombreAdministrador = "AdminPrueba",
            };

            _mediatorMock
               .Setup(m => m.Send(It.IsAny<ConsultarAdministradoresPorIdQuery>(), default))
               .ReturnsAsync(expectedResponse);

            var loggerMock = _loggerMock;

            // Act
            var result = await _administradorController.ConsultarAdministradoresPorIdQuery(id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResponse = Assert.IsType<AdministradorResponse>(okObjectResult.Value);
            Assert.Equal(expectedResponse, actualResponse);
        }

        [Fact]
        public async Task ConsultarAdministradoresPorIdQuery_DeberiaRetornarBadRequestObjectResultCuandoOcurreExcepcion()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedErrorMessage = "Ocurrió un error en ConsultarAdministradoresPorIdQuery de AdministradorController.";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarAdministradoresPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error de prueba"));
            var loggerMock = _loggerMock;
            var administradorController = _administradorController;

            // Act
            var result = await administradorController.ConsultarAdministradoresPorIdQuery(id);

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
            var expectedRequest = new AdministradorUpdateRequest
            {
                Id = expectedId,
                NombreUsuario = "Usuario1",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreAdministrador = "AdminPrueba",

            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ActualizarAdministradorCommand>(), default))
                .ReturnsAsync(expectedId);

            // Act
            var result = await _administradorController.Update(expectedId, expectedRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedId, okResult.Value);
        }


        [Fact]
        public async Task Update_DeberiaRetornarBadRequestSiIdsNoCoinciden()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var request = new AdministradorUpdateRequest
            {
                Id = Guid.NewGuid(),
                NombreUsuario = "Usuario1",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreAdministrador = "AdminPrueba",

            };

            // Act
            var result = await _administradorController.Update(expectedId, request);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
        // Add more test methods here

        [Fact]
        public async Task Update_DeberiaRetornarBadRequestObjectResultCuandoOcurreExcepcion()
        {
            // Arrange
            var id = Guid.NewGuid();
            var consumidorUpdateRequest = new AdministradorUpdateRequest { Id = id };
            var expectedErrorMessage = "Ocurrió un error en AdministradorUpdaterequest de AdministradorController.";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ActualizarAdministradorCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error de prueba"));
            var loggerMock = _loggerMock;
            var administradorController = _administradorController;

            // Act
            var result = await administradorController.Update(id, null);

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

