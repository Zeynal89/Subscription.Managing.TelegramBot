namespace Subscription.Managing.TelegramBot.Application.Contracts.ServiceDetails.Models.Commands;

public class CreateServiceDetailCommand : IRequest
{
    public Duration Duration { get; set; }
    public double Cost { get; set; }
    public int ServiceId { get; set; }
}
