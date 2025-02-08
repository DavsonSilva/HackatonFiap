namespace Hackaton.Domain.Settings;

public class RabbitMQSettings
{
    public string? HostName { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Port { get; set; }
    public string Queue { get; set; }
}