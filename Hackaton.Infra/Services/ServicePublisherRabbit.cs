using System.Text;
using Hackaton.Domain.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Hackaton.Infra.Services;

public class ServicePublisherRabbit
{
    private readonly RabbitMQSettings _rabbitMqSetting;

    public ServicePublisherRabbit(IOptions<RabbitMQSettings> rabbitMqSetting)
    {
        _rabbitMqSetting = rabbitMqSetting.Value;
    }

    public async Task PublishMessageAsync<TEntity>(TEntity message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSetting.HostName,
            UserName = _rabbitMqSetting.UserName,
            Password = _rabbitMqSetting.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        ConfiguraQueue(channel);
        var messageJson = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(messageJson);

        var properties = channel.CreateBasicProperties();
        properties.ContentType = "application/json";
        properties.Type = typeof(TEntity).AssemblyQualifiedName;

        await Task.Run(() =>
            channel.BasicPublish(exchange: "", routingKey: _rabbitMqSetting.Queue, basicProperties: properties,
                body: body));
    }

    private void ConfiguraQueue(IModel channel)
    {
        // Declarar o Dead Letter Exchange (DLX)
        channel.ExchangeDeclare("dead_letter_exchange", ExchangeType.Direct);

        // Declarar a fila Dead Letter Queue (DLQ)
        channel.QueueDeclare(queue: "dead_letter_queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        // Bind da fila de Dead Letter ao Dead Letter Exchange (DLX)
        channel.QueueBind(queue: "dead_letter_queue",
            exchange: "dead_letter_exchange",
            routingKey: "dead_letter_routing_key");

        // Declarar a fila principal com as configurações de Dead Letter
        var arguments = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", "dead_letter_exchange" },
            { "x-dead-letter-routing-key", "dead_letter_routing_key" },
            { "x-message-ttl", 1000000000 }
        };


        channel.QueueDeclare(queue: _rabbitMqSetting.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: arguments);
    }
}