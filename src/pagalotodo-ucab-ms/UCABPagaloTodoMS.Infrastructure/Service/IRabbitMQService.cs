using RabbitMQ.Client;

namespace UCABPagaloTodoMS.Infrastructure.Services
{
    public interface IRabbitMQService
    {
        IModel CreateModel();
        void Dispose();
        void PublishToQueue1(byte[] messageBody);
        
    }
}