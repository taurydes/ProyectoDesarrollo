
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

namespace UCABPagaloTodoMS.Tests.UnitTests.Commands
{
    public class AdministradorCommandTests
    {
        private readonly AdministradorController _administradorController;
        private readonly Mock<ILogger<AdministradorController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<AdministradorRequest>> _administradorValidatorMock;
        private readonly Mock<IValidator<AdministradorUpdateRequest>> _administradorUpdateValidatorMock;
        private readonly IUCABPagaloTodoDbContext _dbcontext; // nueva
        private readonly Mock<IUCABPagaloTodoDbContext> _dbcontextMock;
        private readonly Mock<IDbContextTransactionProxy> _mockProxy;
        private readonly AgregarAdministradorCommandHandler _handler;
        private readonly ActualizarAdministradorCommandHandler _handlerActualizar;
        public AdministradorCommandTests()
        {
            _loggerMock = new Mock<ILogger<AdministradorController>>();
            _mediatorMock = new Mock<IMediator>();
            _administradorController = new AdministradorController(_loggerMock.Object, _mediatorMock.Object, _dbcontext);
            _administradorValidatorMock = new Mock<IValidator<AdministradorRequest>>();
            
            _dbcontextMock = new Mock<IUCABPagaloTodoDbContext>();
            _mockProxy = new Mock<IDbContextTransactionProxy>();
            _handler = new AgregarAdministradorCommandHandler(_dbcontextMock.Object, Mock.Of<ILogger<AgregarAdministradorCommandHandler>>());
            _handlerActualizar = new ActualizarAdministradorCommandHandler(_dbcontextMock.Object, Mock.Of<ILogger<ActualizarAdministradorCommandHandler>>());
        }

       

        [Fact]
        public async Task AgregarAdministradorCommandHandler_Handle_ValidRequestd()
        {
            // Arrange
            var request = new AgregarAdministradorCommand(new AdministradorRequest()
            {
                Id = Guid.NewGuid(),
                NombreUsuario = "Administrador5",
                Clave = "123456789",
                Correo = "test5@gmail.com",
                NombreAdministrador = "AdminPrueba",
            });
          
            var servicio = UsuarioMapper.MapRequestAdministradorEntity(request.Administrador);
            _dbcontextMock.Setup(s => s.Usuarios.Add(servicio));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);
            var result = await _handler.Handle(request, default);
            // Assert
            Assert.IsType<Guid>(result);
        }

        [Fact]
        public async Task AgregarAdministradorCommandHandler_Handle_ExceptionThrown()
        {
            // Arrange
            var request = new AgregarAdministradorCommand(new AdministradorRequest()
            {
                Id = Guid.NewGuid(),
                NombreUsuario = "Administrador5",
                Clave = "123456789",
                Correo = "test5@gmail.com",
                NombreAdministrador = "AdminPrueba",
            });
           
            var servicio = UsuarioMapper.MapRequestAdministradorEntity(request.Administrador);

            _dbcontextMock.Setup(s => s.Usuarios.Add(servicio));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ThrowsAsync(new Exception("Error al guardar el usuario"));

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(request, default));
        }

        [Fact]
        public async Task AgregarAdministradorCommandHandler_Handle_ExceptionThrownInHandle()
        {
            // Arrange
            var request = new AgregarAdministradorCommand(new AdministradorRequest()
            {
                Id = Guid.NewGuid(),
                NombreUsuario = "Administrador5",
                Clave = "123456789",
                Correo = "test5@gmail.com",
                NombreAdministrador = "AdminPrueba",
            });
          
            _dbcontextMock.Setup(s => s.Usuarios.Add(It.IsAny<Usuario>()));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(request, default));
        }

        [Fact]
        public async Task AgregarAdministradorCommandHandler_Handle_NullRequest()
        {
            // Arrange
            var request = new AgregarAdministradorCommand(null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(request, default));
            Assert.Equal("Value cannot be null. (Parameter 'request')", ex.Message);
        }

        // ACTUALIZAR COMMAND 

        [Fact]
        public async Task ActualizarAdministradorCommandHandler_Handle_ValidRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new ActualizarAdministradorCommand(new AdministradorUpdateRequest()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = id,
                NombreUsuario = "ADMINISTRADOR_ACTUALIZADO",
                Clave = "123456789",
                Correo = "admin@ucab.com",
                NombreAdministrador = "Administrador Actualizado"
            }, id);

            var administrador = new Administrador()
            {
                Id = id,
                NombreUsuario = "ADMINISTRADOR_ANTIGUO",
                Clave = "123456789",
                Correo = "admin@ucab.com",
                NombreAdministrador = "Administrador Antiguo"
            };

            _dbcontextMock.Setup(s => s.Administradores.FindAsync(request.Id)).ReturnsAsync(administrador); 
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);
          
             // Act
               var result = await _handlerActualizar.Handle(request, default);

            // Assert
            result.Should().NotBe(default(Guid).ToString());
            administrador.NombreUsuario.Should().Be(request.Administrador.NombreUsuario);
            administrador.Clave.Should().Be(request.Administrador.Clave);
            administrador.Correo.Should().Be(request.Administrador.Correo);
            administrador.NombreAdministrador.Should().Be(request.Administrador.NombreAdministrador);
        }


        // Prueba para el caso en que entra y no puede realizar el commit, xq no se guardar los cambios y no se puede establecer conexión
        [Fact]
        public async Task ActualizarAdministradorCommandHandler_Handle_EntityNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new ActualizarAdministradorCommand(new AdministradorUpdateRequest()
            {
                // Agrega acá los datos que quiers que use la prueba unitaria
                Id = id,
                NombreUsuario = "ADMINISTRADOR_ACTUALIZADO",
                Clave = "123456789",
                Correo = "admin@ucab.com",
                NombreAdministrador = "Administrador Actualizado"
            }, id);

            var administrador = new Administrador()
            {
                Id = id,
                NombreUsuario = "ADMINISTRADOR_ANTIGUO",
                Clave = "123456789",
                Correo = "admin@ucab.com",
                NombreAdministrador = "Administrador Antiguo"
            };

           _dbcontextMock.Setup(s => s.Administradores.FindAsync(request.Id)).ReturnsAsync(administrador);

            // Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handlerActualizar.Handle(request, default));
           
        }

        [Fact]
        public async Task Handle_WhenAdminNotFound_ShouldThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new ActualizarAdministradorCommand(new AdministradorUpdateRequest()
            {
                Id = id,
                NombreUsuario = "ADMINISTRADOR_ACTUALIZADO",
                Clave = "123456789",
                Correo = "admin@ucab.com",
                NombreAdministrador = "Administrador Actualizado"
            }, id);

            // Configurar el DbContext Mock
            _dbcontextMock.Setup(s => s.Administradores.FindAsync(request.Id)).ThrowsAsync(new Exception("Error al buscar el administrador en la base de datos"));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);
            _dbcontextMock.Setup(s => s.BeginTransaction()).Returns(_mockProxy.Object);
           
            // Verificar que se haya llamado a Rollback en la transacción
            _dbcontextMock.Setup(s => s.BeginTransaction().Rollback());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => _handlerActualizar.Handle(request, CancellationToken.None));

        }



       
    }

}

