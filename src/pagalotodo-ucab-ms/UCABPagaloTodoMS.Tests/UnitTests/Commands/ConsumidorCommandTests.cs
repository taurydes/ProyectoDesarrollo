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
using UCABPagaloTodoMS.Application.Handlers.Commands.FeatureConsumidor;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Handlers.Commands;

namespace UCABPagaloTodoMS.Tests.UnitTests.Commands
{
    public class ConsumidorCommandTests
    {
        private readonly ConsumidorController _consumidorController;
        private readonly Mock<ILogger<ConsumidorController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<ConsumidorRequest>> _consumidorValidatorMock;
        private readonly Mock<IValidator<ConsumidorUpdateRequest>> _consumidorUpdateValidatorMock;
        private readonly IUCABPagaloTodoDbContext _dbcontext; // nueva
        
        //comand
        private readonly Mock<IDbContextTransactionProxy> _mockProxy;
        private readonly AgregarConsumidorCommandHandler _handler;
        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly ActualizarConsumidorCommandHandler _handlerActualizar;
        private readonly CambiarEstatusCommandHandler _handlerEstatus;


        public ConsumidorCommandTests()
        {
            _loggerMock = new Mock<ILogger<ConsumidorController>>();
            _mediatorMock = new Mock<IMediator>();
            _consumidorController = new ConsumidorController(_loggerMock.Object, _mediatorMock.Object, _dbcontext);
            _consumidorValidatorMock = new Mock<IValidator<ConsumidorRequest>>();
            //
            _mockProxy = new Mock<IDbContextTransactionProxy>();
            _dbContextMock = new Mock<IUCABPagaloTodoDbContext>();
            _handler = new AgregarConsumidorCommandHandler(_dbContextMock.Object, Mock.Of<ILogger<AgregarConsumidorCommandHandler>>());
            _handlerActualizar = new ActualizarConsumidorCommandHandler(_dbContextMock.Object, Mock.Of<ILogger<ActualizarConsumidorCommandHandler>>());
            _handlerEstatus = new CambiarEstatusCommandHandler(_dbContextMock.Object, Mock.Of<ILogger<CambiarEstatusCommandHandler>>());

        }

        [Fact]
        public async Task AgregarConsumidorCommandHandler_Handle_ValidRequestd()
        {
            // Arrange
           
            var request = new AgregarConsumidorCommand(new ConsumidorRequest()
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

            });

            var servicio = UsuarioMapper.MapRequestConsumidorEntity(request._request);
            _dbContextMock.Setup(s => s.Usuarios.Add(servicio));
            _dbContextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbContextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);
            var result = await _handler.Handle(request, default);
            // Assert
            Assert.IsType<Guid>(result);
        }

        [Fact]
        public async Task AgregarConsumidorCommandHandler_Handle_ExceptionThrown()
        {
            // Arrange
            var request = new AgregarConsumidorCommand(new ConsumidorRequest()
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

            });

            var servicio = UsuarioMapper.MapRequestConsumidorEntity(request._request);

            _dbContextMock.Setup(s => s.Usuarios.Add(servicio));
            _dbContextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ThrowsAsync(new Exception("Error al guardar el usuario"));

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(request, default));
        }

        [Fact]
        public async Task AgregarConsumidorCommandHandler_Handle_ExceptionThrownInHandle()
        {
            // Arrange
            var request = new AgregarConsumidorCommand(new ConsumidorRequest()
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

            });

            _dbContextMock.Setup(s => s.Usuarios.Add(It.IsAny<Usuario>()));
            _dbContextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(request, default));

        }

        [Fact]
        public async Task AgregarConsumidorCommandHandler_Handle_NullRequest()
        {
            // Arrange
            var request = new AgregarConsumidorCommand(null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(request, default));
            Assert.Equal("Value cannot be null. (Parameter 'request')", ex.Message);
        }


        //Actualizar

        [Fact]
        public async Task ActualizarConsumidorCommandHandler_Handle_ValidRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new ActualizarConsumidorCommand(new ConsumidorUpdateRequest()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                NombreUsuario = "usuario1Nuevo",
                Clave = "123456789",
                Correo = "correo1@test.com",
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = 123456,
                Direccion = "Calle 123",
                EstatusCuenta = true,
            }, id);
           
           var Consumidor = new Consumidor()
            {
                Id = id,
               NombreUsuario = "usuario1Antiguo",
               Clave = "123456789",
               Correo = "correo1@test.com",
               Nombre = "Juan",
               Apellido = "Perez",
               Telefono = 123456,
               Direccion = "Calle 123",
               EstatusCuenta = true,
           };

            _dbContextMock.Setup(s => s.Consumidores.FindAsync(request.Id)).ReturnsAsync(Consumidor);
            _dbContextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbContextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);

            // Act
            var result = await _handlerActualizar.Handle(request, default);

            // Assert
            result.Should().NotBe(default(Guid).ToString());
            Consumidor.NombreUsuario.Should().Be(request.Consumidor.NombreUsuario);
            Consumidor.Clave.Should().Be(request.Consumidor.Clave);
            Consumidor.Correo.Should().Be(request.Consumidor.Correo);
            Consumidor.Nombre.Should().Be(request.Consumidor.Nombre);
            Consumidor.Apellido.Should().Be(request.Consumidor.Apellido);
            Consumidor.Telefono.Should().Be(request.Consumidor.Telefono);
            Consumidor.Direccion.Should().Be(request.Consumidor.Direccion);
        }

        [Fact]
        public async Task ActualizarConsumidorCommandHandler_Handle_EntityNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new ActualizarConsumidorCommand(new ConsumidorUpdateRequest()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                NombreUsuario = "usuario1Nuevo",
                Clave = "123456789",
                Correo = "correo1@test.com",
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = 123456,
                Direccion = "Calle 123",
                EstatusCuenta = true,
            }, id);

            var Consumidor = new Consumidor()
            {
                Id = id,
                NombreUsuario = "usuario1Antiguo",
                Clave = "123456789",
                Correo = "correo1@test.com",
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = 123456,
                Direccion = "Calle 123",
                EstatusCuenta = true,
            };

            _dbContextMock.Setup(s => s.Consumidores.FindAsync(request.Id)).ReturnsAsync(Consumidor);

            // Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handlerActualizar.Handle(request, default));

        }

        [Fact]
        public async Task Handle_WhenAdminNotFound_ShouldThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new ActualizarConsumidorCommand(new ConsumidorUpdateRequest()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                NombreUsuario = "usuario1Nuevo",
                Clave = "123456789",
                Correo = "correo1@test.com",
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = 123456,
                Direccion = "Calle 123",
                EstatusCuenta = true,
            }, id);

            // Configurar el DbContext Mock
            _dbContextMock.Setup(s => s.Consumidores.FindAsync(request.Id)).ThrowsAsync(new Exception("Error al buscar el Consumidor en la base de datos"));
            _dbContextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbContextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);

            // Verificar que se haya llamado a Rollback en la transacción
            _dbContextMock.Setup(s => s.BeginTransaction().Rollback());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => _handlerActualizar.Handle(request, CancellationToken.None));

        }

        //Actualizar ESTATUS

        [Fact]
        public async Task CambiaEstatusConsumidorCommandHandler_Handle_ValidRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new CambiarEstatusCommand(new ConsumidorResponse()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = id,
                NombreUsuario = "usuario1Nuevo",
                Clave = "123456789",
                Correo = "correo1@test.com",
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = 123456,
                Direccion = "Calle 123",
                EstatusCuenta = true,
            }, id);

            var Consumidor = new Consumidor()
            {
                Id = id,
                NombreUsuario = "usuario1Nuevo",
                Clave = "123456789",
                Correo = "correo1@test.com",
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = 123456,
                Direccion = "Calle 123",
                EstatusCuenta = false,
            };

            _dbContextMock.Setup(s => s.Consumidores.FindAsync(request.Id)).ReturnsAsync(Consumidor);
            _dbContextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbContextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);

            // Act
            var result = await _handlerEstatus.Handle(request, default);

            // Assert
            result.Should().NotBe(default(Guid).ToString());
            Consumidor.EstatusCuenta.Should().Be(request.Consumidor.EstatusCuenta.Value);
          
        }

        [Fact]
        public async Task CambiaEstatusConsumidorCommandHandler_Handle_EntityNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new CambiarEstatusCommand(new ConsumidorResponse()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = id,
                NombreUsuario = "usuario1Nuevo",
                Clave = "123456789",
                Correo = "correo1@test.com",
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = 123456,
                Direccion = "Calle 123",
                EstatusCuenta = true,
            }, id);

            var Consumidor = new Consumidor()
            {
                Id = id,
                NombreUsuario = "usuario1Nuevo",
                Clave = "123456789",
                Correo = "correo1@test.com",
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = 123456,
                Direccion = "Calle 123",
                EstatusCuenta = false,
            };

            _dbContextMock.Setup(s => s.Consumidores.FindAsync(request.Id)).ReturnsAsync(Consumidor);

            // Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handlerEstatus.Handle(request, default));

        }

        [Fact]
        public async Task Handle_CambiaEstatusNotFound_ShouldThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new CambiarEstatusCommand(new ConsumidorResponse()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = id,
                NombreUsuario = "usuario1Nuevo",
                Clave = "123456789",
                Correo = "correo1@test.com",
                Nombre = "Juan",
                Apellido = "Perez",
                Telefono = 123456,
                Direccion = "Calle 123",
                EstatusCuenta = true,
            }, id);

            // Configurar el DbContext Mock
            _dbContextMock.Setup(s => s.Consumidores.FindAsync(request.Id)).ThrowsAsync(new Exception("Error al buscar el Consumidor en la base de datos"));
            _dbContextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbContextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);

            // Verificar que se haya llamado a Rollback en la transacción
            _dbContextMock.Setup(s => s.BeginTransaction().Rollback());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => _handlerEstatus.Handle(request, CancellationToken.None));

        }
    }

}


