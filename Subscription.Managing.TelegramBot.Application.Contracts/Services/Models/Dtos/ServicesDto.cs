namespace Subscription.Managing.TelegramBot.Application.Contracts.Services.Models.Dtos;

public class ServicesDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ServiceStatus Status { get; set; }
    public string BotLink { get; set; }
}
