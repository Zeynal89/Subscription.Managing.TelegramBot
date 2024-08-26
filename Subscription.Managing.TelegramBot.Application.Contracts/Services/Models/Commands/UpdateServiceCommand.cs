namespace Subscription.Managing.TelegramBot.Application.Contracts.Services.Models.Commands;

public class UpdateServiceCommand : IRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ServiceStatus Status { get; set; }
    public DateTime EndDate { get; set; }
    public string BotLink { get; set; }
}
