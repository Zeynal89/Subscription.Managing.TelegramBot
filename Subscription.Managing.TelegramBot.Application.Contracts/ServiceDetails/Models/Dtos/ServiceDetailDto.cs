namespace Subscription.Managing.TelegramBot.Application.Contracts.ServiceDetails.Models.Dtos;

public class ServiceDetailDto
{
    public int Id { get; set; }
    public Duration Duration { get; set; }
    public double Cost { get; set; }
    public int ServiceId { get; set; }
}
