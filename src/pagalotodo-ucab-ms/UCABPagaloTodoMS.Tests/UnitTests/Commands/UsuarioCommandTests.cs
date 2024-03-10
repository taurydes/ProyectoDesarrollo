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
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Application.Commands.FeatureServiciosPrestados;
using UCABPagaloTodoMS.Application.Handlers;

namespace UCABPagaloTodoMS.Tests.UnitTests.Commands
{
    public class UsuarioCommandTests
    {
        private readonly UsuarioController _usuarioController;
        private readonly Mock<ILogger<UsuarioController>> _loggerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly IJwtService _jwtService;// nueva
        private readonly IMapper _mapper;
        private readonly IConfiguration _config; // Agrega esta línea 

        private readonly Mock<IUCABPagaloTodoDbContext> _dbcontextMock;
        private readonly Mock<IDbContextTransactionProxy> _mockProxy;
        private readonly EliminarUsuarioPorIdCommandHandler _handlerEliminar;
   

        public UsuarioCommandTests()
        {
            _loggerMock = new Mock<ILogger<UsuarioController>>();
            _mediatorMock = new Mock<IMediator>();
            _usuarioController = new UsuarioController(_loggerMock.Object, _mediatorMock.Object, _jwtService, _mapper, _config);
            _mockConfig = new Mock<IConfiguration>();
            _mockJwtService = new Mock<IJwtService>();

            _dbcontextMock = new Mock<IUCABPagaloTodoDbContext>();
            _mockProxy = new Mock<IDbContextTransactionProxy>();
            _handlerEliminar = new EliminarUsuarioPorIdCommandHandler(_dbcontextMock.Object, Mock.Of<ILogger<EliminarUsuarioPorIdCommandHandler>>());
           
        }

        //ELIMINAR
        [Fact]
        public async Task Handle_Usuario_FuncionaELiminayReturnId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new EliminarUsuarioCommand(id);
            var servicio = new Usuario { Id = id };
            _dbcontextMock.Setup(s => s.Usuarios.FindAsync(id)).ReturnsAsync(servicio);
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(true);

            // Act
            var result = await _handlerEliminar.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(id, result);
            _dbcontextMock.Verify(s => s.Usuarios.FindAsync(id), Times.Once);
            _dbcontextMock.Verify(s => s.Usuarios.Remove(servicio), Times.Once);
            _dbcontextMock.Verify(s => s.SaveEfContextChanges("APP", default), Times.Once);
        }

        [Fact]
        public async Task GeneraExepcióncuandoOcurreUnError()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new EliminarUsuarioCommand(id);
            var servicio = new Usuario { Id = id };
            _dbcontextMock.Setup(s => s.Usuarios.FindAsync(id)).ThrowsAsync(new Exception("Error al buscar el Usuario en la base de datos"));
            _dbcontextMock.Setup(s => s.SaveEfContextChanges("APP", default)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handlerEliminar.Handle(command, CancellationToken.None));

        }




    }

}
