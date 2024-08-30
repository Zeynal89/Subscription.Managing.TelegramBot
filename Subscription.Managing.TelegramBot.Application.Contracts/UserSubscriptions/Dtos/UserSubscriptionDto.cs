namespace Subscription.Managing.TelegramBot.Application.Contracts.UserSubscriptions.Dtos;

public class UserSubscriptionDto
{
    public int Id { get; set; }
    public UserDto User { get; set; }
    public UserSubscriptionServiceDetailDto ServiceDetail { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public UserSubscriptionStatus UserSubscriptionStatus { get; set; }
}
