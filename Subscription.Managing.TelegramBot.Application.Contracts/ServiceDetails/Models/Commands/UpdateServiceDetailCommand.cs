namespace Subscription.Managing.TelegramBot.Application.Contracts.ServiceDetails.Models.Commands;

public class UpdateServiceDetailCommand : IRequest
{
    public int Id { get; set; }
    public Duration Duration { get; set; }
    public double Cost { get; set; }
    public int ServiceId { get; set; }
}
