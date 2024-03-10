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
using UCABPagaloTodoMS.Application.Commands;
using Microsoft.AspNetCore.Routing;
using AutoMapper;
using UCABPagaloTodoMS.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using static Microsoft.Azure.Amqp.Serialization.SerializableType;


namespace UCABPagaloTodoMS.Tests.UnitTests.Controllers
{
    public class UsuarioControllerTests
    {
        private readonly UsuarioController _usuarioController;
        private readonly Mock<ILogger<UsuarioController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly IJwtService _jwtService;// nueva
        private readonly IMapper _mapper;
        private readonly IConfiguration _config; // Agrega esta línea 


        public UsuarioControllerTests()
        {
            _loggerMock = new Mock<ILogger<UsuarioController>>();
            _mediatorMock = new Mock<IMediator>();
            _usuarioController = new UsuarioController(_loggerMock.Object, _mediatorMock.Object, _jwtService, _mapper, _config);
            _mockConfig = new Mock<IConfiguration>();
            _mockJwtService = new Mock<IJwtService>();
        }

        [Fact]
        public async Task Delete_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<EliminarUsuarioCommand>(), default)).Verifiable();

            // Act
            var result = await _usuarioController.Delete(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("usuario eliminado", okResult.Value);

            _mediatorMock.Verify();
        }

        [Fact]
        public async void Delete_Returns_BadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<EliminarUsuarioCommand>(), default))
                .Throws(new Exception("Error de prueba"));

            // Act
            var result = await _usuarioController.Delete(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Ocurrió un error en Delete de UsuarioController: {StatusCodes.Status400BadRequest}", badRequestResult.Value);
        }



        [Fact]
        public async Task ConsultarUsuarioQuery_RetornaListaDeUsuarioResponse()
        {
            // Arrange
            var expectedResponse = new List<UsuarioResponse>
            {
                new UsuarioResponse
                {
                    Id = Guid.NewGuid(),
                    NombreUsuario = "usuario1",
                    Clave = "clave123456",
                    Correo = "correo1@test.com",

                },
                new UsuarioResponse
                {
                    Id = Guid.NewGuid(),
                    NombreUsuario = "usuario2",
                    Clave = "clave123456",
                    Correo = "correo2@test.com",
                }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarUsuariosQuery>(), default))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _usuarioController.ConsultarUsuariosQuery();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResponse = Assert.IsType<List<UsuarioResponse>>(okObjectResult.Value);

        }

        [Fact]
        public async void ConsultarUsuariosQuery_Throws_Exception_Returns_BadRequest()
        {
            // Arrange

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarUsuariosQuery>(), default))
                .Throws(new Exception("Error de prueba"));

            // Act
            var result = await _usuarioController.ConsultarUsuariosQuery();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Ocurrió un error en ConsultarUsuariosQuery de UsuarioController.", badRequestResult.Value);
        }



        [Fact]
        public async Task ConsultarUsuarioPorIdQuery_RetornaOk200ActionResultConUsuarioResponse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResponse = new UsuarioResponse
            {
                Id = id,
                NombreUsuario = "usuario1",
                Clave = "clave123456",
                Correo = "correo1@test.com",

            };

            _mediatorMock
               .Setup(m => m.Send(It.IsAny<ConsultarUsuarioPorIdQuery>(), default))
               .ReturnsAsync(expectedResponse);


            // Act
            var result = await _usuarioController.ConsultarUsuarioPorIdQueryHandler(id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResponse = Assert.IsType<UsuarioResponse>(okObjectResult.Value);
            Assert.Equal(expectedResponse, actualResponse);
        }

        [Fact]
        public async Task ConsultarUsuarioPorIdQuery_DeberiaRetornarBadRequestObjectResultCuandoOcurreExcepcion()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedErrorMessage = "Ocurrió un error en GetById de UsuarioController.";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ConsultarUsuarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error de prueba"));


            // Act
            var result = await _usuarioController.ConsultarUsuarioPorIdQueryHandler(id);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestObjectResult.StatusCode);
            var errorMessage = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        // acá agregamos las otras pruebas unitarias que creamos 


        //LOGIN 

        //[Fact]
        //public async Task Login_Validar_Returns_Token()
        //{
        //    // Arrange
        //    var id = Guid.NewGuid();
        //    var usuarioResponse = new UsuarioResponse
        //    {
        //        Id = id,
        //        NombreUsuario = "usuario1",
        //        Clave = "123456",
        //        Correo = "test@gmail.com"
        //    };
        //    var loginRequest = new LoginRequest
        //    {
        //        NombreUsuario = "usuario1",
        //        Clave = "123456"
        //    };

        //    var jwtSecret = "mi_clave_secreta";
        //    _mockConfig.Setup(c => c["JwtOptions:Secret"]).Returns(jwtSecret);
        //    _mockJwtService.Setup(s => s.GenerateJwtToken(It.IsAny<Usuario>())).Returns("token");

        //    // Simulamos que el usuario se encuentra en la base de datos y la contraseña es correcta
        //    _mediatorMock
        //        .Setup(m => m.Send(It.IsAny<ConsultarUsuarioPorNombreQuery>(), default))
        //        .ReturnsAsync(usuarioResponse);

        //    // Act
        //    var result = await _usuarioController.Login(loginRequest);
        //    // Assert
        //    // Verificar que se devuelva un resultado de OkObjectResult
        //    var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        //    var token = Assert.IsType<string>(okObjectResult.Value);
        //    Assert.Equal(token, okObjectResult.Value);

        //}


    }

}
