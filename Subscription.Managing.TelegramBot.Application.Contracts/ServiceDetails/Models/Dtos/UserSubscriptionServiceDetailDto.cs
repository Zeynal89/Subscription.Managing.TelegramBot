namespace Subscription.Managing.TelegramBot.Application.Contracts.ServiceDetails.Models.Dtos;

public class UserSubscriptionServiceDetailDto
{
    public int Id { get; set; }
    public Duration Duration { get; set; }
    public double Cost { get; set; }
    public ServiceDto ServiceDto { get; set; }
}
