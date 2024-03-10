using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UCABPagaloTodoMS.Application.Requests;
using UCABPagaloTodoMS.Core.Database;


namespace UCABPagaloTodoMS.Infrastructure.Services
{
    /// <summary>
    /// clase que controla la cola de mensajes recibidos
    /// </summary>
    public class RabbitMqConsumerConciliacionHS : IHostedService
    {
        private readonly string _queueName;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IServiceProvider _serviceProvider;
    
        /// <summary>
        /// constructor de la clase donde se inicializan los objetos necesarios
        /// </summary>
        /// <param name="serviceProvider"></param>
        public RabbitMqConsumerConciliacionHS(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _queueName = "conciliacion";

            _channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _cancellationTokenSource = new CancellationTokenSource();
        }
        /// <summary>
        /// método que inicia el servidor rabbit MQ
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, args) =>
            {
                var message = Encoding.UTF8.GetString(args.Body.ToArray());
                var request = JsonSerializer.Deserialize<ActualizaEstadosPagoRabbitRequest>(message);
                await UpdateStatusPago(request);
                Console.WriteLine("Received message: " + message);

                _channel.BasicAck(args.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumer: consumer
            );

            // Ejecuta el consumidor en segundo plano
            Task.Run(() => ConsumeMessagesAsync(_cancellationTokenSource.Token));

            return Task.CompletedTask;
        }

        /// <summary>
        /// método que detiene el servidor de colas Rabbit
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            _channel.Close();
            _connection.Close();

            return Task.CompletedTask;
        }
        public async Task UpdateStatusPago(ActualizaEstadosPagoRabbitRequest  datosActualizar)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IUCABPagaloTodoDbContext>();
                if (datosActualizar.Estado is null) datosActualizar.Estado = "vacio";
                using (var transaccion = dbContext.BeginTransaction())
                {
                    try
                    {
                        
                        if (datosActualizar.Estado.ToLower() != ("aprobado") && datosActualizar.Estado != ("rechazado")) datosActualizar.Estado="pendiente";

                       var entity = await dbContext.Pagos.FirstOrDefaultAsync(u => u.Referencia == datosActualizar.Referencia);
                       
                        if (entity is null)
                        {
                            throw new ArgumentNullException(nameof(entity));
                        }

                        entity.EstadoPago = datosActualizar.Estado  ?? "pendiente";

                        dbContext.Pagos.Update(entity);
                        dbContext.DbContext.SaveChanges();
                        await dbContext.SaveEfContextChanges("APP");
                        transaccion.Commit();
                    }
                    catch (Exception)
                    {
                        transaccion.Rollback();
                    }
                }
            }

        }

        /// <summary>
        /// método que consume el mensaje
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ConsumeMessagesAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Ejecuta el consumo de mensajes en un bucle mientras no se haya cancelado
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
