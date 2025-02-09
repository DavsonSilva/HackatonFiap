using System.Reflection;
using Hackaton.ConsumerRabbit;
using Hackaton.Domain.Settings;
using Hackaton.Infra.Ioc;
using SendGrid.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSendGrid(options =>
{
    options.ApiKey = builder.Configuration
    .GetSection("SendGridEmailSettings").GetValue<string>("APIKey");
});

builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection(nameof(RabbitMQSettings)));

builder.Services.ConfigureConsumer(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var host = builder.Build();
host.Run();