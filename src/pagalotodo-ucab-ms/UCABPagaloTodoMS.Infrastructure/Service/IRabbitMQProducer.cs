namespace UCABPagaloTodoMS.Infrastructure.Services
{
    public interface IRabbitMQProducer
    {
        void PublishMessage(string message);
        void PublishMessageToConciliacion_Queue(string message);
    }
}