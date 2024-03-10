
using Microsoft.Extensions.Logging;
using Moq;
using UCABPagaloTodoMS.Controllers;
using Xunit;
using UCABPagaloTodoMS.Application.Queries;
using UCABPagaloTodoMS.Application.Responses;
using UCABPagaloTodoMS.Core.Entities;
using UCABPagaloTodoMS.Core.Database;
using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Application.Mappers;
using UCABPagaloTodoMS.Application.Handlers.Queries;
//using MockQueryable.EntityFrameworkCore;


namespace UCABPagaloTodoMS.Tests.UnitTests.Querys

{
    public class AdministradorQueryTests
    {
        private readonly Mock<ILogger<AdministradorController>> _loggerMock;

        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly ConsultarAdministradoresQueryHandler _handler;
        private readonly ConsultarAdministradoresPorIdQueryHandler _handlerPorId;
        private readonly Mock<DbSet<Administrador>> _mockSet;

        public AdministradorQueryTests()
        {
            _loggerMock = new Mock<ILogger<AdministradorController>>();

            _dbContextMock = new Mock<IUCABPagaloTodoDbContext>();
            _handler = new ConsultarAdministradoresQueryHandler(_dbContextMock.Object, Mock.Of<ILogger<ConsultarAdministradoresQueryHandler>>());
            _handlerPorId = new ConsultarAdministradoresPorIdQueryHandler(_dbContextMock.Object, Mock.Of<ILogger<ConsultarAdministradoresPorIdQueryHandler>>());
            _mockSet = new Mock<DbSet<Administrador>>();
        }

      
        [Fact]
        public async Task ConsultarAdministradoresQueryHandler_Handle_ReturnsAdministradorResponseList()
        {
            // Arrange
            var administradores = new List<Administrador>
            {
                new Administrador { Id = Guid.NewGuid(), NombreAdministrador = "Administrador 1" },
                new Administrador { Id = Guid.NewGuid(), NombreAdministrador = "Administrador 2" },
            };

            var responseList = administradores.Select(entity => UsuarioMapper.MapEntityAdministradorAResponse(entity)).ToList();

            //_mockSet.As<IAsyncEnumerable<Administrador>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            //    .Returns(new TestAsyncEnumerator<Administrador>(administradores.GetEnumerator()));

            _mockSet.As<IQueryable<Administrador>>().Setup(m => m.Expression).Returns(administradores.AsQueryable().Expression);
            _mockSet.As<IQueryable<Administrador>>().Setup(m => m.ElementType).Returns(administradores.AsQueryable().ElementType);
            _mockSet.As<IQueryable<Administrador>>().Setup(m => m.GetEnumerator()).Returns(administradores.GetEnumerator());

            _dbContextMock.Setup(m => m.Administradores).Returns(_mockSet.Object);     
          

            var query = new ConsultarAdministradoresQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None); // prueba el handler 

            // Assert
            Assert.NotNull(result); // verifica que sea no null 
            Assert.IsType<List<AdministradorResponse>>(result); // verifica que se responda una lisa de administradoresResponse
            Assert.Equal(administradores.Count, result.Count); // Verifica que la longitud de la lista es la esperada

        }


        //prueba unitaria que verifica el flujo OK de una consulta de administrador por ID
       [Fact]
        public async Task HandleAsync_WithValidId_ReturnsAdministradorResponse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new ConsultarAdministradoresPorIdQuery(id);
            var administrador = new Administrador { Id = id, NombreAdministrador = "Administrador 1" };

            _dbContextMock.Setup(s => s.Administradores.FindAsync(administrador.Id)).ReturnsAsync(administrador);



            // Act
            var result = await _handlerPorId.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AdministradorResponse>(result);
            Assert.Equal(administrador.NombreAdministrador, result.NombreAdministrador);
        }
    }
}

               
    


