using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EventBusService.Messaging
{
    public class RabbitMqService:IRabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        // Constructor: Initializes RabbitMQ connection using configuration
        public RabbitMqService()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        // Publishes a message to the specified exchange with a routing key
        public void Publish<T>(T message, string exchange, string routingKey)
        {
            _channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange, routingKey, properties, body);
        }

        // Subscribes to a queue and handles incoming messages
        public void Subscribe<T>(string queue, string exchange, string routingKey, Action<T> handler)
        {
            _channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);
            _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue, exchange, routingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                handler(message);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue, false, consumer);
        }

        // Disposes RabbitMQ channel and connection
        public void Dispose()
        {
            _channel?.Dispose();
            _connection.Dispose();
        }

    }
}
