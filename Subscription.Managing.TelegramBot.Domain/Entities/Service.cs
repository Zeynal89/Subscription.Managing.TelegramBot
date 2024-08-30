namespace Subscription.Managing.TelegramBot.Domain.Entities;

public class Service
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ServiceStatus Status { get; set; }
    public string? BotLink { get; set; }
    public List<ServiceDetail>? ServiceDetails { get; set; }
}
