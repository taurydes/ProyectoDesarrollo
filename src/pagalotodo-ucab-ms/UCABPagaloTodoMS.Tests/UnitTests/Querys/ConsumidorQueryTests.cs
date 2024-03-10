
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
using UCABPagaloTodoMS.Tests.DataSeed;

namespace UCABPagaloTodoMS.Tests.UnitTests.Querys

{
    public class ConsumidorQueryTests
    {
        private readonly Mock<ILogger<ConsumidorController>> _loggerMock;

        private readonly Mock<IUCABPagaloTodoDbContext> _dbContextMock;
        private readonly ConsultarConsumidoresQueryHandler _handler;
        private readonly ConsultarConsumidoresPorIdQueryHandler _handlerPorId;
        private readonly Mock<DbSet<Consumidor>> _mockSet;

        public ConsumidorQueryTests()
        {
            _loggerMock = new Mock<ILogger<ConsumidorController>>();

            _dbContextMock = new Mock<IUCABPagaloTodoDbContext>();
            _handler = new ConsultarConsumidoresQueryHandler(_dbContextMock.Object, Mock.Of<ILogger<ConsultarConsumidoresQueryHandler>>());
            _handlerPorId = new ConsultarConsumidoresPorIdQueryHandler(_dbContextMock.Object, Mock.Of<ILogger<ConsultarConsumidoresPorIdQueryHandler>>());
            _mockSet = new Mock<DbSet<Consumidor>>();
        }

      
        [Fact]
        public async Task ConsultarConsumidoresQueryHandler_Handle_ReturnsConsumidorResponseList()
        {
            // Arrange
            var consumidores = new List<Consumidor>
            {
                new Consumidor { Id = Guid.NewGuid(), Nombre = "Consumidor 1" },
                new Consumidor { Id = Guid.NewGuid(), Nombre = "Consumidor 2" },
            };

            var responseList = consumidores.Select(entity => UsuarioMapper.MapEntityConsumidorAResponse(entity)).ToList();

            //_mockSet.As<IAsyncEnumerable<Consumidor>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            //    .Returns(new TestAsyncEnumerator<Consumidor>(consumidores.GetEnumerator()));

            _mockSet.As<IQueryable<Consumidor>>().Setup(m => m.Expression).Returns(consumidores.AsQueryable().Expression);
            _mockSet.As<IQueryable<Consumidor>>().Setup(m => m.ElementType).Returns(consumidores.AsQueryable().ElementType);
            _mockSet.As<IQueryable<Consumidor>>().Setup(m => m.GetEnumerator()).Returns(consumidores.GetEnumerator());

            _dbContextMock.Setup(m => m.Consumidores).Returns(_mockSet.Object);     
          

            var query = new ConsultarConsumidoresQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None); // prueba el handler 

            // Assert
            Assert.NotNull(result); // verifica que sea no null 
            Assert.IsType<List<ConsumidorResponse>>(result); // verifica que se responda una lisa de ConsumidoresResponse
            Assert.Equal(consumidores.Count, result.Count); // Verifica que la longitud de la lista es la esperada
            _dbContextMock.SetupDbContextData();
        }


        //prueba unitaria que verifica el flujo OK de una consulta de Consumidor por ID
        //[Fact]
        //public async Task HandleAsync_WithValidId_ReturnsConsumidorResponse()
        //{
        //    // Arrange
        //    var id = Guid.NewGuid();
        //    var consumidor = new Consumidor
        //    {
        //        Id = id,
        //        Nombre = "John",
        //        Apellido = "Doe",
        //        PagosRealizados = new List<Pago> { new Pago { Id = Guid.NewGuid(), Monto = 200 } }
        //    };

        //    var consumidores = new List<Consumidor> { consumidor };

        //    _mockSet.As<IQueryable<Consumidor>>().Setup(m => m.Provider).Returns(consumidores.AsQueryable().Provider);
        //    _mockSet.As<IQueryable<Consumidor>>().Setup(m => m.Expression).Returns(consumidores.AsQueryable().Expression);
        //    _mockSet.As<IQueryable<Consumidor>>().Setup(m => m.ElementType).Returns(consumidores.AsQueryable().ElementType);
        //    _mockSet.As<IQueryable<Consumidor>>().Setup(m => m.GetEnumerator()).Returns(consumidores.AsQueryable().GetEnumerator());
        //    _mockSet.Setup(m => m.FindAsync(id)).ReturnsAsync(consumidor);

        //    _dbContextMock.Setup(db => db.Consumidores).Returns(_mockSet.Object);

        //    // Act
        //    var result = await _handlerPorId.Handle(new ConsultarConsumidoresPorIdQuery(id), CancellationToken.None);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(consumidor.Nombre, result.Nombre);
        //    Assert.Equal(consumidor.Apellido, result.Apellido);
        //    Assert.Equal(consumidor.PagosRealizados.Count, result.PagosRealizados.Count);
        //}
    }
}

               
    


