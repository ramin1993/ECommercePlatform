namespace EventBusService.Messaging
{
    public interface IRabbitMqService:IDisposable
    {
        void Publish<T>(T message, string exchange, string routingKey);
        void Subscribe<T>(string queue, string exchange, string routingKey, Action<T> handler);
    }
}
