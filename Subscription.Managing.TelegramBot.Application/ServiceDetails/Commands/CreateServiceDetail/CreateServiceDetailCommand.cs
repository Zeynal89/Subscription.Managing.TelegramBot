namespace Subscription.Managing.TelegramBot.Application.ServiceDetails.Commands.CreateServiceDetail;

public class CreateServiceDetailCommand : IRequest
{
    public Duration Duration { get; set; }
    public double Cost { get; set; }
    public int ServiceId { get; set; }
}
