namespace Subscription.Managing.TelegramBot.Application.ServiceDetails.Commands.UpdateServiceDetail;

public class UpdateServiceDetailCommand : IRequest
{
    public int Id { get; set; }
    public Duration Duration { get; set; }
    public double Cost { get; set; }
    public int ServiceId { get; set; }
}
