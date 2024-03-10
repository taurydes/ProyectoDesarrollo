using Microsoft.Extensions.Logging;
using Moq;
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
using ValidationResult = FluentValidation.Results.ValidationResult;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeaturePrestadorServicio;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Application.Handlers.Commands.FeaturePrestadorServicio;
using UCABPagaloTodoMS.Application.Mappers;

namespace UCABPagaloTodoMS.Tests.UnitTests.Commands
{
    public class PrestadorServicioCommandTests
    {
        private readonly PrestadorServicioController _prestadorController;
        private readonly Mock<ILogger<PrestadorServicioController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<PrestadorServicioRequest>> _prestadorValidatorMock;
        private readonly Mock<IValidator<PrestadorServicioUpdateRequest>> _prestadorUpdateValidatorMock;
        private readonly IUCABPagaloTodoDbContext _dbcontext; // nueva
        private readonly Mock<IUCABPagaloTodoDbContext> _dbcontextMock;
        private readonly Mock<IDbContextTransactionProxy> _mockProxy;
        private readonly AgregarPrestadorServicioCommandHandler _handler;
        private readonly ActualizarPrestadorServicioCommandHandler _handlerActualizar;
        private readonly CambiarEstatusCommandHandler _handlerEstatus;


        public PrestadorServicioCommandTests()
        {
            _loggerMock = new Mock<ILogger<PrestadorServicioController>>();
            _mediatorMock = new Mock<IMediator>();
            _prestadorController = new PrestadorServicioController(_loggerMock.Object, _mediatorMock.Object, _dbcontext);
            _prestadorValidatorMock = new Mock<IValidator<PrestadorServicioRequest>>();

            _dbcontextMock = new Mock<IUCABPagaloTodoDbContext>();
            _mockProxy = new Mock<IDbContextTransactionProxy>();
            _handler = new AgregarPrestadorServicioCommandHandler(_dbcontextMock.Object, Mock.Of<ILogger<AgregarPrestadorServicioCommandHandler>>());
            _handlerActualizar = new ActualizarPrestadorServicioCommandHandler(_dbcontextMock.Object, Mock.Of<ILogger<ActualizarPrestadorServicioCommandHandler>>());
            _handlerEstatus = new CambiarEstatusCommandHandler(_dbcontextMock.Object, Mock.Of<ILogger<CambiarEstatusCommandHandler>>());

        }

        [Fact]
        public async Task AgregarPrestadorServicioCommandHandler_Handle_ValidRequestd()
        {
            // Arrange
            var request = new AgregarPrestadorServicioCommand(new PrestadorServicioRequest()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = Guid.NewGuid(),
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789"
              
            });

            var servicio = UsuarioMapper.MapRequestPrestadorServicioEntity(request.Prestador);
            _dbcontextMock.Setup(s => s.Usuarios.Add(servicio));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);
            var result = await _handler.Handle(request, default);
            // Assert
            Assert.IsType<Guid>(result);
        }

        [Fact]
        public async Task AgregarPrestadorServicioCommandHandler_Handle_ExceptionThrown()
        {
            // Arrange
            var request = new AgregarPrestadorServicioCommand(new PrestadorServicioRequest()
            {
                Id = Guid.NewGuid(),
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789"
            });

            var servicio = UsuarioMapper.MapRequestPrestadorServicioEntity(request.Prestador);

            _dbcontextMock.Setup(s => s.Usuarios.Add(servicio));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ThrowsAsync(new Exception("Error al guardar el usuario"));

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(request, default));
        }

        [Fact]
        public async Task AgregarPrestadorServicioCommandHandler_Handle_ExceptionThrownInHandle()
        {
            // Arrange
            var request = new AgregarPrestadorServicioCommand(new PrestadorServicioRequest()
            {
                Id = Guid.NewGuid(),
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789"
            });

            _dbcontextMock.Setup(s => s.Usuarios.Add(It.IsAny<Usuario>()));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(request, default));

        }

        [Fact]
        public async Task AgregarPrestadorServicioCommandHandler_Handle_NullRequest()
        {
            // Arrange
            var request = new AgregarPrestadorServicioCommand(null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(request, default));
            Assert.Equal("Value cannot be null. (Parameter 'request')", ex.Message);
        }

        //ACtulizar

        [Fact]
        public async Task ActualizarPrestadorServicioCommandHandler_Handle_ValidRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new ActualizarPrestadorServicioCommand(new PrestadorServicioUpdateRequest()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = id,
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789"
            }, id);

            var PrestadorServicio = new PrestadorServicio()
            {
                Id = id,
                NombreUsuario = "ANTIGUO",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789"
            };

            _dbcontextMock.Setup(s => s.PrestadorServicios.FindAsync(request.Id)).ReturnsAsync(PrestadorServicio);
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);

            // Act
            var result = await _handlerActualizar.Handle(request, default);

            // Assert
            result.Should().NotBe(default(Guid).ToString());
            PrestadorServicio.NombreUsuario.Should().Be(request.Prestador.NombreUsuario);
            PrestadorServicio.Clave.Should().Be(request.Prestador.Clave);
            PrestadorServicio.Correo.Should().Be(request.Prestador.Correo);
            PrestadorServicio.NombreEmpresa.Should().Be(request.Prestador.NombreEmpresa);
            PrestadorServicio.Rif.Should().Be(request.Prestador.Rif);
        }

        // Prueba para el caso en que entra y no puede realizar el commit, xq no se guardar los cambios y no se puede establecer conexión
        [Fact]
        public async Task ActualizarAdministradorCommandHandler_Handle_EntityNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new ActualizarPrestadorServicioCommand(new PrestadorServicioUpdateRequest()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = id,
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789"
            }, id);

            var PrestadorServicio = new PrestadorServicio()
            {
                Id = id,
                NombreUsuario = "ANTIGUO",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789"
            };

            _dbcontextMock.Setup(s => s.PrestadorServicios.FindAsync(request.Id)).ReturnsAsync(PrestadorServicio);

            // Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handlerActualizar.Handle(request, default));

        }

        [Fact]
        public async Task Handle_WhenAdminNotFound_ShouldThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new ActualizarPrestadorServicioCommand(new PrestadorServicioUpdateRequest()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = id,
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789"
            }, id);

            // Configurar el DbContext Mock
            _dbcontextMock.Setup(s => s.PrestadorServicios.FindAsync(request.Id)).ThrowsAsync(new Exception("Error al buscar el administrador en la base de datos"));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);

            // Verificar que se haya llamado a Rollback en la transacción
            _dbcontextMock.Setup(s => s.BeginTransaction().Rollback());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => _handlerActualizar.Handle(request, CancellationToken.None));

        }

        //Actualizar ESTATUS

        [Fact]
        public async Task CambiaEstatusPrestadorCommandHandler_Handle_ValidRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new CambiarEstatusCommand(new PrestadorServicioResponse()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = id,
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789",
                EstatusCuenta = true,
            }, id);

            var prestador = new PrestadorServicio()
            {
                Id = id,
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789",
                EstatusCuenta = false,
            };

            _dbcontextMock.Setup(s => s.PrestadorServicios.FindAsync(request.Id)).ReturnsAsync(prestador);
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);

            // Act
            var result = await _handlerEstatus.Handle(request, default);

            // Assert
            result.Should().NotBe(default(Guid).ToString());
            prestador.EstatusCuenta.Should().Be(request.prestador.EstatusCuenta.Value);

        }

        [Fact]
        public async Task CambiaEstatusConsumidorCommandHandler_Handle_EntityNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new CambiarEstatusCommand(new PrestadorServicioResponse()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = id,
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789",
                EstatusCuenta = true,
            }, id);

            var prestador = new PrestadorServicio()
            {
                Id = id,
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789",
                EstatusCuenta = false,
            };

            _dbcontextMock.Setup(s => s.PrestadorServicios.FindAsync(request.Id)).ReturnsAsync(prestador);

            // Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handlerEstatus.Handle(request, default));

        }

        [Fact]
        public async Task Handle_CambiaEstatusNotFound_ShouldThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new CambiarEstatusCommand(new PrestadorServicioResponse()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = id,
                NombreUsuario = "ACTUALIZANDO2",
                Clave = "123456789",
                Correo = "daniel@gmail.com",
                NombreEmpresa = "ACTUALIZANDO",
                Rif = "V123456789",
                EstatusCuenta = true,
            }, id);

            // Configurar el DbContext Mock
            _dbcontextMock.Setup(s => s.PrestadorServicios.FindAsync(request.Id)).ThrowsAsync(new Exception("Error al buscar el Prestador en la base de datos"));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);

            // Verificar que se haya llamado a Rollback en la transacción
            _dbcontextMock.Setup(s => s.BeginTransaction().Rollback());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => _handlerEstatus.Handle(request, CancellationToken.None));

        }
    }
}

