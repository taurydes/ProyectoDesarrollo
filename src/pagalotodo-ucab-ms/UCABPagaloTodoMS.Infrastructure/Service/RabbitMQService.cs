using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
//using UCABPagaloTodoMS.Core.Services;

namespace UCABPagaloTodoMS.Infrastructure.Services
{
    public class RabbitMQService : IDisposable, IRabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queue1Name;
        private readonly string _queue2Name;

        public RabbitMQService(IConfiguration configuration)
        {
            //datos por default de rabbit
            /*"Host": "localhost",
            "Port": 5672,
            "Username": "guest",
            "Password": "guest"*/
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Nombres de las colas
            _queue1Name = "conciliacion";

            // Declarar y crear las colas si no existen
            DeclareQueue(_queue1Name);
          
        }

        private void DeclareQueue(string queueName)
        {
            _channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        public IModel CreateModel()
        {
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        // Métodos adicionales para trabajar con las colas específicas

        public void PublishToQueue1(byte[] messageBody)
        {
            _channel.BasicPublish(
                exchange: string.Empty,
                routingKey: _queue1Name,
                basicProperties: null,
                body: messageBody
            );
        }

       
    }
}
