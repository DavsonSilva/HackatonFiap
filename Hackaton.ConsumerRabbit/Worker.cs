using System.Text;
using System.Text.Json;
using Hackaton.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Hackaton.ConsumerRabbit;

public class Worker : BackgroundService
{
    private readonly RabbitMQSettings _rabbitMqSetting;
    private readonly IMediator _mediator;

    public Worker(IOptions<RabbitMQSettings> rabbitMqSetting, IMediator mediator)
    {
        _mediator = mediator;
        _rabbitMqSetting = rabbitMqSetting.Value;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSetting.HostName,
            UserName = _rabbitMqSetting.UserName,
            Password = _rabbitMqSetting.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = channel.BasicGet(queue: _rabbitMqSetting.Queue, autoAck: false);

            if (result != null)
            {
                var body = result.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                string typeMessage = result.BasicProperties.Type;
                var type = Type.GetType(typeMessage);
                if (type is null)
                    continue;
                
                try
                {
                    var typedMessage = (IRequest) JsonSerializer.Deserialize(message, type)!;
                    await _mediator.Send(typedMessage, stoppingToken);
                    channel.BasicAck(deliveryTag: result.DeliveryTag, multiple: false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    channel.BasicNack(result.DeliveryTag, false, false); 
                    continue;
                }
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}