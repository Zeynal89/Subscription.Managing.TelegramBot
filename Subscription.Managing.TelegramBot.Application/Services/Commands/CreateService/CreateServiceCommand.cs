namespace Subscription.Managing.TelegramBot.Application.Services.Commands.CreateService;

public class CreateServiceCommand : IRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ServiceStatus Status { get; set; }
    public DateTime EndDate { get; set; }
    public string BotLink { get; set; }
}
