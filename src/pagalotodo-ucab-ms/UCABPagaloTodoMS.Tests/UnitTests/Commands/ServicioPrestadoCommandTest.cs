using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using UCABPagaloTodoMS.Application.Commands;
using UCABPagaloTodoMS.Application.Commands.FeatureServicioPrestado;
using UCABPagaloTodoMS.Application.Commands.FeatureServiciosPrestados;
using UCABPagaloTodoMS.Application.Handlers.Commands.FeatureServiciosPrestados;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Controllers;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using Xunit;

namespace UCABPagaloTodoMS.Tests.UnitTests.Commands
{
    public class ServicioPrestadoCommandTest
    {
        private readonly ServicioPrestadoController _servicioController;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<ServicioPrestadoController>> _loggerMock;
        private readonly Mock<IValidator<CrearServicioPrestadoRequest>> _validatorMock;

        private readonly Mock<IUCABPagaloTodoDbContext> _dbcontextMock;
        private readonly Mock<IDbContextTransactionProxy> _mockProxy;
        private readonly CrearServicioPrestadoCommandHandler _handler;
        private readonly ActualizarServicioPrestadoCommandHandler _handlerActualizar;
        private readonly EliminarServicioPrestadoPorIdCommandHandler _handlerEliminar;
        private readonly CambiarEstatusCommandHandler _handlerEstatus;

        public ServicioPrestadoCommandTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<ServicioPrestadoController>>();
            _validatorMock = new Mock<IValidator<CrearServicioPrestadoRequest>>();
            _servicioController = new ServicioPrestadoController(_loggerMock.Object, _mediatorMock.Object);

            _dbcontextMock = new Mock<IUCABPagaloTodoDbContext>();
            _mockProxy = new Mock<IDbContextTransactionProxy>();
            _handler = new CrearServicioPrestadoCommandHandler(_dbcontextMock.Object, Mock.Of<ILogger<CrearServicioPrestadoCommandHandler>>());
            _handlerActualizar = new ActualizarServicioPrestadoCommandHandler(_dbcontextMock.Object, Mock.Of<ILogger<ActualizarServicioPrestadoCommandHandler>>());
            _handlerEliminar = new EliminarServicioPrestadoPorIdCommandHandler(_dbcontextMock.Object, Mock.Of<ILogger<EliminarServicioPrestadoPorIdCommandHandler>>());
            _handlerEstatus = new CambiarEstatusCommandHandler(_dbcontextMock.Object, Mock.Of<ILogger<CambiarEstatusCommandHandler>>());

        }


        [Fact]
        public async Task AgregarServicioCommandHandler_Handle_Valido()
        {
            // Arrange
            var request = new CrearServicioPrestadoCommand(new CrearServicioPrestadoRequest()
            {
                Id = Guid.NewGuid(),
                Nombre = "Servicio de prueba",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,
                PrestadorServicioId = Guid.NewGuid(),
            });

            var servicio = ServicioPrestadoMapper.MapRequestServicioPrestadoEntity(request.Servicio);
            _dbcontextMock.Setup(s => s.ServiciosPrestados.Add(servicio));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);
            var result = await _handler.Handle(request, default);
            // Assert
            Assert.IsType<Guid>(result);
        }

        [Fact]
        public async Task AgregarServicioPrestadoCommandHandler_Handle_ExceptionThrown()
        {
            // Arrange
            var request = new CrearServicioPrestadoCommand(new CrearServicioPrestadoRequest()
            {
                Id = Guid.NewGuid(),
                Nombre = "Servicio de prueba",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,
                PrestadorServicioId = Guid.NewGuid(),
            });

            var servicio = ServicioPrestadoMapper.MapRequestServicioPrestadoEntity(request.Servicio);

            _dbcontextMock.Setup(s => s.ServiciosPrestados.Add(servicio));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ThrowsAsync(new Exception("Error al guardar el Servicio"));

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(request, default));
        }

        [Fact]
        public async Task AgregarServicioPrestadoCommandHandler_Handle_ExceptionThrownInHandle()
        {
            // Arrange
            var request = new CrearServicioPrestadoCommand(new CrearServicioPrestadoRequest()
            {
                Id = Guid.NewGuid(),
                Nombre = "Servicio de prueba",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,
                PrestadorServicioId = Guid.NewGuid(),
            });

            _dbcontextMock.Setup(s => s.ServiciosPrestados.Add(It.IsAny<ServicioPrestado>()));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(request, default));
        }

        [Fact]
        public async Task AgregarServicioPrestadoCommandHandler_Handle_NullRequest()
        {
            // Arrange
            var request = new CrearServicioPrestadoCommand(null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(request, default));
            Assert.Equal("Value cannot be null. (Parameter 'request')", ex.Message);
        }

       
        // ACTUALIZAR COMMAND 

        [Fact]
        public async Task ActualizarServicioPrestadoCommandHandler_Handle_ValidRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new ActualizarServicioPrestadoCommand(new ServicioPrestadoUpdateRequest()
            {
                Id = id,
                Nombre = "Servicio Actualizado",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,
                
            }, id);

            var ServicioPrestado = new ServicioPrestado()
            {
                Id = id,
                Nombre = "Servicio Viejo ",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,
            };

            _dbcontextMock.Setup(s => s.ServiciosPrestados.FindAsync(request.Id)).ReturnsAsync(ServicioPrestado);
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);

            // Act
            var result = await _handlerActualizar.Handle(request, default);

            // Assert
            result.Should().NotBe(default(Guid).ToString());
            ServicioPrestado.Nombre.Should().Be(request.Servicio.Nombre);
            ServicioPrestado.Descripcion.Should().Be(request.Servicio.Descripcion);
            ServicioPrestado.EstadoServicio.Should().Be(request.Servicio.EstadoServicio);
            ServicioPrestado.TipoPago.Should().Be(request.Servicio.TipoPago);
        }


        // Prueba para el caso en que entra y no puede realizar el commit, xq no se guardar los cambios y no se puede establecer conexión
        [Fact]
        public async Task ActualizarServicioPrestadoCommandHandler_Handle_EntityNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new ActualizarServicioPrestadoCommand(new ServicioPrestadoUpdateRequest()
            {
                Id = id,
                Nombre = "Servicio Actualizado",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,

            }, id);

            var ServicioPrestado = new ServicioPrestado()
            {
                Id = id,
                Nombre = "Servicio Viejo ",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,
            };

            _dbcontextMock.Setup(s => s.ServiciosPrestados.FindAsync(request.Id)).ReturnsAsync(ServicioPrestado);

            // Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handlerActualizar.Handle(request, default));

        }

        [Fact]
        public async Task Handle_CuandoNoFuncionaYGeneraExepción()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new ActualizarServicioPrestadoCommand(new ServicioPrestadoUpdateRequest()
            {
                Id = id,
                Nombre = "Servicio Actualizado",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,

            }, id);

            // Configurar el DbContext Mock
            _dbcontextMock.Setup(s => s.ServiciosPrestados.FindAsync(request.Id)).ThrowsAsync(new Exception("Error al buscar el ServicioPrestado en la base de datos"));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);

            // Verificar que se haya llamado a Rollback en la transacción
            _dbcontextMock.Setup(s => s.BeginTransaction().Rollback());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => _handlerActualizar.Handle(request, CancellationToken.None));

        }

        //ELIMINAR
        [Fact]
        public async Task Handle_ServicioPrestado_FuncionaELiminayReturnId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new EliminarServicioPrestadoCommand(id);
            var servicio = new ServicioPrestado { Id = id };
            _dbcontextMock.Setup(s => s.ServiciosPrestados.FindAsync(id)).ReturnsAsync(servicio);
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
         
            // Act
            var result = await _handlerEliminar.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(id, result);
            _dbcontextMock.Verify(s => s.ServiciosPrestados.FindAsync(id), Times.Once);
            _dbcontextMock.Verify(s => s.ServiciosPrestados.Remove(servicio), Times.Once);
            _dbcontextMock.Verify(s => s.SaveEfContextChanges("APP", default), Times.Once);
        }

        [Fact]
        public async Task GeneraExepcióncuandoOcurreUnaexepción()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new EliminarServicioPrestadoCommand(id);
            var servicio = new ServicioPrestado { Id = id };
            _dbcontextMock.Setup(s => s.ServiciosPrestados.FindAsync(id)).ThrowsAsync(new Exception("Error al buscar el Servicio en la base de datos")); 
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(false);
            
            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handlerEliminar.Handle(command, CancellationToken.None));
                      
        }

        //Actualizar ESTATUS

        [Fact]
        public async Task CambiaEstatusPrestadorCommandHandler_Handle_ValidRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new CambiarEstatusCommand(new ServicioPrestadoResponse()

            {
                Id = id,
                Nombre = "Servicio Actualizado",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,
                EstatusServicio = true,

            }, id);

            var ServicioPrestado = new ServicioPrestado()
            {
                Id = id,
                Nombre = "Servicio Viejo ",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,
                EstatusServicio = false,

            };

            _dbcontextMock.Setup(s => s.ServiciosPrestados.FindAsync(request.Id)).ReturnsAsync(ServicioPrestado);
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);

            // Act
            var result = await _handlerEstatus.Handle(request, default);

            // Assert
            result.Should().NotBe(default(Guid).ToString());
            ServicioPrestado.EstatusServicio.Should().Be(request.Servicio.EstatusServicio);

        }

        [Fact]
        public async Task CambiaEstatusConsumidorCommandHandler_Handle_EntityNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new CambiarEstatusCommand(new ServicioPrestadoResponse()

            {
                Id = id,
                Nombre = "Servicio Actualizado",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,
                EstatusServicio = true,

            }, id);

            var ServicioPrestado = new ServicioPrestado()
            {
                Id = id,
                Nombre = "Servicio Viejo ",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,
                EstatusServicio = false,

            };

            _dbcontextMock.Setup(s => s.ServiciosPrestados.FindAsync(request.Id)).ReturnsAsync(ServicioPrestado);

            // Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handlerEstatus.Handle(request, default));

        }

        [Fact]
        public async Task Handle_CambiaEstatusNotFound_ShouldThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new CambiarEstatusCommand(new ServicioPrestadoResponse()

            {
                Id = id,
                Nombre = "Servicio Actualizado",
                Descripcion = "Descripción del servicio de prueba",
                Costo = 10.0f,
                EstadoServicio = "activo",
                TipoPago = true,
                EstatusServicio = true,

            }, id);

            // Configurar el DbContext Mock
            _dbcontextMock.Setup(s => s.ServiciosPrestados.FindAsync(request.Id)).ThrowsAsync(new Exception("Error al buscar el servicio en la base de datos"));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);

            // Verificar que se haya llamado a Rollback en la transacción
            _dbcontextMock.Setup(s => s.BeginTransaction().Rollback());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => _handlerEstatus.Handle(request, CancellationToken.None));

        }
    }

}
