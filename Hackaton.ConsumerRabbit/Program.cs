using System.Reflection;
using Hackaton.ConsumerRabbit;
using Hackaton.Infra.Ioc;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.ConfigureApplicationContext(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var host = builder.Build();
host.Run();