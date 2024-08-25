namespace Subscription.Managing.TelegramBot.Application.ServiceDetails.Dtos;

public class ServiceDetailDto
{
    public int Id { get; set; }
    public Duration Duration { get; set; }
    public double Cost { get; set; }
    public ServiceDto Service { get; set; }
}
