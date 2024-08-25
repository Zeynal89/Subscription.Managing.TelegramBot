namespace Subscription.Managing.TelegramBot.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Username { get; set; }
    public List<UserSubscription>? UserSubscriptions { get; set; }
}
